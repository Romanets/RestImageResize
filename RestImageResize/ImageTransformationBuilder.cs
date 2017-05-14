using System;
using System.Globalization;
using OpenWaves.ImageTransformations;
using RestImageResize.Contracts;
using RestImageResize.Transformations;
using RestImageResize.Utils;

namespace RestImageResize
{
    /// <summary>
    /// The default implementation if <see cref="IImageTransformationBuilder"/> component.
    /// </summary>
    public class ImageTransformationBuilder
    {
        private const string PropertiesStringSeparator = ";";
        private const string ValueSholdBePositiveOrZero = "Value should be positive or 0.";

        private int _height;
        private ImageTransform _transformType;
        private int _width;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageTransformationBuilder"/> class.
        /// </summary>
        public ImageTransformationBuilder()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageTransformationBuilder"/> class.
        /// </summary>
        /// <param name="serializedProperties">The serialized properties.</param>
        /// <remarks>This constructor is used by <see cref="ImageTransformationParser"/> component.</remarks>
        /// <exception cref="System.ArgumentNullException">serializedProperties</exception>
        /// <exception cref="System.ArgumentException">Bad format.;serializedProperties</exception>
        public ImageTransformationBuilder(string serializedProperties)
        {
            if (serializedProperties == null)
            {
                throw new ArgumentNullException("serializedProperties");
            }

            string[] propertyValues = serializedProperties.Split(new[] { PropertiesStringSeparator }, StringSplitOptions.RemoveEmptyEntries);
            bool badFormat = propertyValues.Length != 3;

            if (!badFormat)
            {
                try
                {
                    TransformType = SmartConvert.ChangeType<ImageTransform>(propertyValues[0]);
                    Width = SmartConvert.ChangeType<int>(propertyValues[1]);
                    Height = SmartConvert.ChangeType<int>(propertyValues[2]);
                }
                catch
                {
                    badFormat = true;
                }
            }

            if (badFormat)
            {
                throw new ArgumentException("Bad format.", "serializedProperties");
            }
        }

        /// <summary>
        /// Gets the image transformation.
        /// </summary>
        /// <value>
        /// The image transformation.
        /// </value>
        protected IImageTransformation ImageTransformation { get; private set; }

        /// <summary>
        /// Gets or sets the desired image width.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">value</exception>
        public int Width
        {
            get { return _width; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", ValueSholdBePositiveOrZero);
                }

                if (value != _width)
                {
                    _width = value;
                    ImageTransformation = null;
                }

            }
        }

        /// <summary>
        /// Gets or sets the desired image height.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">value</exception>
        public int Height
        {
            get { return _height; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", ValueSholdBePositiveOrZero);
                }

                if (value != _height)
                {
                    _height = value;
                    ImageTransformation = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the type of the transformation that should be applied in image.
        /// </summary>
        public ImageTransform TransformType
        {
            get { return _transformType; }
            set
            {
                if (value != _transformType)
                {
                    _transformType = value;
                    ImageTransformation = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the image focus point.
        /// This point will be as close to the center of your crop as possible while keeping the crop within the image
        /// </summary>
        public FocusPoint FocusPoint { get; set; }

        /// <summary>
        /// Applies transformation to an image.
        /// </summary>
        /// <param name="image">The image.</param>
        public void ApplyToImage(IImage image)
        {
            PrepareSize(image);
            throw new NotImplementedException();
            ImageTransformation.ApplyToImage(image);
        }

        /// <summary>
        /// Serializes applied transformation instance.
        /// </summary>
        /// <returns>
        /// The serialization string.
        /// </returns>
        public virtual string Serialize()
        {
            if (ImageTransformation != null)
            {
                return ImageTransformation.Serialize();
            }

            return string.Format(CultureInfo.InvariantCulture, "{0}|{1}", GetType().AssemblyQualifiedName, SerializeProperties());
        }

        /// <summary>
        /// Serializes the properties.
        /// </summary>
        /// <returns>
        /// Serialized string.
        /// </returns>
        protected virtual string SerializeProperties()
        {
            return string.Join(PropertiesStringSeparator, TransformType, Width, Height);
        }

        /// <summary>
        /// Prepares the transformation size before transformation performing.
        /// </summary>
        /// <param name="image">The target image.</param>
        protected virtual void PrepareSize(IImage image)
        {
            bool ignoreMissedDimension = (TransformType == ImageTransform.Fit || TransformType == ImageTransform.DownFit);

            if (TransformType == ImageTransform.ResizeMin || TransformType == ImageTransform.DownResizeMin)
            {
                if (Width == 0)
                {
                    Width = Height;
                }

                if (Height == 0)
                {
                    Height = Width;
                }
            }


            if (Width == 0)
            {
                Width = ignoreMissedDimension ? int.MaxValue : image.Width;
            }

            if (Height == 0)
            {
                Height = ignoreMissedDimension ? int.MaxValue : image.Height;
            }
        }

        ///// <summary>
        ///// Creates the image transformation to be applied.
        ///// </summary>
        ///// <returns>
        ///// The transformation instance.
        ///// </returns>
        ///// <exception cref="System.NotSupportedException">Not supported image transformation type.</exception>
        //protected virtual IImageTransformation CreateTransformation()
        //{
        //    int width = Width;
        //    int height = Height;
        //    switch (TransformType)
        //    {
        //        case ImageTransform.Fit:
        //            return new ScaleToFitTransformation(width, height);
        //        case ImageTransform.Fill:
        //            return new ScaleToFillTransformation(width, height);
        //        case ImageTransform.DownFit:
        //            return new ScaleDownToFitTransformation(width, height);
        //        case ImageTransform.DownFill:
        //            return new ScaleDownToFillTransformation(width, height);
        //        case ImageTransform.Crop:
        //            return new CentralCropTransformation(width, height);
        //        case ImageTransform.Stretch:
        //            return new StretchTransformation(width, height);
        //        case ImageTransform.ResizeMin:
        //            return new ResizeMinTransformation(width, height);
        //        case ImageTransform.DownResizeMin:
        //            return new DownResizeMinTransformation(width, height);
        //        case ImageTransform.ResizeCrop:
        //            return new ResizeCropTransfomration(FocusPoint, width, height);

        //        default:
        //            throw new NotSupportedException("Not supported image transformation type.");
        //    }
        //}

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Serialize();
        }
    }
}
