//>>built
define("epi/cms/contentediting/editors/PreviewableDateTimeEditor",["dojo/_base/declare","dojo/dom-construct","epi/datetime","epi/shell/widget/DateTimeSelectorDropDown","epi/cms/contentediting/editors/_PreviewableEditor"],function(_1,_2,_3,_4,_5){return _1("epi.cms.contentediting.editors.PreviewableDateTimeEditor",[_5],{required:false,controlParams:["required"],buildRendering:function(){this.control=new _4({datePackage:null});this.inherited(arguments);},onChange:function(_6){this.set("labelValue",_3.toUserFriendlyHtml(_6));}});});