//>>built
define("epi/cms/widget/overlay/overlayFactory",["dojo/_base/lang","dojo/_base/Deferred","epi/string"],function(_1,_2,_3){return {defaultType:"epi/cms/widget/overlay/Property",create:function(_4){var _5=new _2();var _6=_4.property,_7=_3.slashName(_4.property.metadata.customEditorSettings.overlayType||this.defaultType);require([_7],_1.hitch(this,function(_8){var _9=_1.mixin({name:_6.name,contentModel:_6.contentModel,displayName:_6.metadata.displayName,disabled:_4.disabled,sourceItemNode:_4.node,dropTargetType:_4.property.metadata.additionalValues.dropTargetType,dropTargetChildProperty:_4.property.metadata.additionalValues.dropTargetChildProperty},_6.overlayParams);var _a=new _8(_9);_5.resolve(_a);}));return _5;}};});