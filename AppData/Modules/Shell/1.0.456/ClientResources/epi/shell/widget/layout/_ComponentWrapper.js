//>>built
require({cache:{"url:epi/shell/widget/layout/templates/_ComponentWrapperHeader.htm":"<div>\r\n    <div class=\"epi-gadgetTitle dojoxDragHandle\" data-dojo-attach-point=\"titleBarNode\">\r\n        <div data-dojo-attach-point=\"focusNode\" class=\"epi-gadgetTitleFocus\">\r\n            <button data-dojo-type=\"dijit.form.ToggleButton\" data-dojo-attach-point=\"toggleButton\"\r\n                data-dojo-attach-event=\"onClick:toggle\" data-dojo-props=\"title:'${res.togglebuttontooltip}', showLabel:true, iconClass:'epi-gadget-toggle', 'class':'epi-chromelessButton'\">\r\n                ${title}</button>\r\n        </div>\r\n        <div class=\"epi-gadgetButtonBar\">\r\n            <button data-dojo-type=\"dijit.form.Button\" data-dojo-attach-point=\"closeButton\" data-dojo-attach-event=\"onClick:onClose\"\r\n                data-dojo-props=\"showLabel:false, title:'${res.closebuttontooltip}', iconClass:'epi-gadget-delete', 'class':'epi-chromelessButton'\">\r\n                ${res.closebuttontooltip}</button>\r\n        </div>\r\n    </div>\r\n</div>\r\n"}});define("epi/shell/widget/layout/_ComponentWrapper",["dojo/_base/declare","dojo/_base/array","dojo/string","dojo/_base/lang","dojo/dom-style","dojo/dom-attr","dojo/dom-construct","dojo/dom-class","dojo/dom-geometry","dojo/cookie","dojo/Evented","dijit/_base/wai","dijit/_CssStateMixin","dijit/_TemplatedMixin","dijit/_WidgetsInTemplateMixin","dijit/_Widget","dijit/layout/BorderContainer","epi/shell/command/_CommandProviderMixin","epi/shell/widget/command/GadgetAction","epi/shell/widget/command/RemoveGadget","epi/shell/widget/ComponentChrome","epi/shell/widget/dialog/Confirmation","epi/shell/widget/layout/_ComponentResizeMixin","epi/shell/widget/layout/_ComponentSplitter","dojo/text!./templates/_ComponentWrapperHeader.htm","epi/i18n!epi/shell/ui/nls/episerver.shell.ui.resources.gadgetchrome","dijit/form/Button","dijit/form/ToggleButton"],function(_1,_2,_3,_4,_5,_6,_7,_8,_9,_a,_b,_c,_d,_e,_f,_10,_11,_12,_13,_14,_15,_16,_17,_18,_19,res){var _1a=_1([_10,_e,_f,_d,_b],{templateString:_19,title:"",res:res,open:true,toggleable:true,closable:false,"class":"epi-gadgetHeader",toggle:function(){},onClose:function(){},postCreate:function(){this.inherited(arguments);if(this.toggleable){this._trackMouseState(this.titleBarNode,"epi-gadgetTitle");}},startup:function(){this.inherited(arguments);this.toggleButton.set("checked",!this.open);},_setClosableAttr:function(_1b){_5.set(this.closeButton.domNode,"display",_1b?"":"none");_8.toggle(this.titleBarNode,"epi-gadget-unlocked",_1b);},_setTitleAttr:function(_1c){this._set("title",_1c||"");this.toggleButton.set("label",this.title);},_setToggleableAttr:function(_1d){this.toggleable=_1d;_c.setWaiRole(this.focusNode,_1d?"button":"heading");if(_1d){_c.setWaiState(this.focusNode,"controls",this.id+"_pane");_6.set(this.focusNode,"tabIndex",this.tabIndex);}else{_6.remove(this.focusNode,"tabIndex");}this.toggleButton.set("disabled",!this.toggleable);this.toggleButton.set("iconClass",this.toggleable?"epi-gadget-toggle":"");}});return _1("epi.shell.widget.layout._ComponentWrapper",[_11,_17,_d,_12,_b],{dndType:"epi.shell.layout._ComponentWrapper",heading:"",toggleable:true,tabIndex:"0",baseClass:"epi-gadget","class":"epi-gadgetContainer",closable:false,dragRestriction:false,confirmationBeforeRemoval:true,res:res,gutters:false,postCreate:function(){this.inherited(arguments);this._header=new _1a({region:"top",splitter:false,title:this.heading||"",closable:this.closable,toggle:_4.hitch(this,this.toggle),onClose:_4.hitch(this,this.onClose)});this.addChild(this._header);},_setIsRemovableAttr:function(_1e){_1e=_1e!==false;this._set("isRemovable",_1e);this.set("closable",(_1e&this.closable?true:false));if(this.isRemovable){this.add("commands",new _14({model:this}));}},_setClosableAttr:function(_1f){this._set("closable",_1f);this._header&&this._header.set("closable",_1f);},_showRemovalConfirmationDialog:function(_20){var _21=new _16({description:_3.substitute(this.res.removecomponentquestion,{name:this.heading}),title:epi.resources.header.episerver,onAction:_20});_21.show();},onClose:function(evt){if(!this.confirmationBeforeRemoval){_5.set(this.domNode,"display","none");this.onClosed();}else{this._showRemovalConfirmationDialog(_4.hitch(this,function(_22){if(_22){_5.set(this.domNode,"display","none");this.onClosed();}}));return true;}},startup:function(){this._header.set("open",this.open);this.set("open",this.open);this.inherited(arguments);},resize:function(_23){_23=_4.mixin({},_23);if(!_23.h){if(!this.open){var _24=_9.getMarginExtents(this.domNode).h;_23.h=_24+this._getClosedHeight();}else{if(this.lastOpenHeight){_23.h=this.lastOpenHeight;}}}this.inherited(arguments,[_23]);},getSize:function(){var _25=_9.getMarginBox(this.domNode);if(!this.open){_25.h=this._getClosedHeight();}return _25;},_getClosedHeight:function(){var _26=this._header?_9.getMarginBox(this._header.domNode).h:0,_27=this._splitter?_9.getMarginBox(this._splitter.domNode).h:0;return _26+_27;},onClosed:function(){},_setHeadingAttr:function(_28){this._set("heading",_28);this._header&&this._header.set("title",_28);},_setToggleableAttr:function(_29){this._header&&this._header.set("toggleable",_29);},toggle:function(){this._started&&this.set("open",!this.get("open"));},_setOpenAttr:function(_2a){this.inherited(arguments);if(this._started){if(!_2a){this.lastOpenHeight=_9.getMarginBox(this.domNode).h;}this.emit("toggle",this.open);}},addChild:function(_2b){if(_2b.isInstanceOf(_18)||_2b.isInstanceOf(_1a)){return this.inherited(arguments);}this._addGadgetCommands(_2b);var _2c=new _15({region:"center",splitter:false});_2c.addProvider(this);_2c.addChild(_2b);this._child=_2c;this.inherited("addChild",[_2c]);if(this._started&&!_2b.started&&!_2b._started){_2b.startup();}},_addGadgetCommands:function(_2d){if(_2d&&_4.isArray(_2d.gadgetActions)){_2.forEach(_2d.gadgetActions,function(_2e){this.add("commands",new _13({actionName:_2e.actionName,label:_2e.text,model:_2d}));},this);}}});});