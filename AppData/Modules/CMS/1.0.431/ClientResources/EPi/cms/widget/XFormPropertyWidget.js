//>>built
define("epi/cms/widget/XFormPropertyWidget",["dojo/_base/declare","dojo/_base/lang","epi/epi","epi/routes","epi/cms/legacy/LegacyDialogWrapper","epi/cms/core/ContentReference","epi/i18n!epi/cms/nls/episerver.cms.widget.xformeditor"],function(_1,_2,_3,_4,_5,_6,_7){return _1("epi.cms.widget.XFormPropertyWidget",[_5],{title:_7.selectform,onChange:function(_8){},postMixInProperties:function(){this.features=_2.mixin({width:800,height:400},this.editorFeatures);this.autoFit=true;this.connect(this,"onCallback","_onCallback");this.inherited(arguments);},startup:function(){this.inherited(arguments);this.set("url",this._buildIframeUrl());},onCancel:function(){},_buildIframeUrl:function(){var _9={moduleArea:"LegacyCMS",path:"edit/XFormSelect.aspx",epslanguage:this.contentLanguage,propertyname:this.fullName};_2.mixin(_9,{"pageId":new _6(this.contentLink).createVersionUnspecificReference().id,"form":this.value?this.value:""});return _4.getActionPath(_9);},_onCallback:function(_a){var _b=_a?_a.id:null;if(!_3.areEqual(this.value,_b)&&_a!==null){this.value=_b;this.onChange(_b);}else{this.onCancel();}}});});