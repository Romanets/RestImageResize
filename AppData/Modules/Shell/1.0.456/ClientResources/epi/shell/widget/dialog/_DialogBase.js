//>>built
require({cache:{"url:epi/shell/widget/dialog/templates/Dialog.html":"<div class=\"dijitDialog\" role=\"dialog\" aria-labelledby=\"${id}_title\">\r\n    <div data-dojo-attach-point=\"titleBar\" class=\"dijitDialogTitleBar\">\r\n        <span data-dojo-attach-point=\"titleNode\" class=\"dijitDialogTitle\" id=\"${id}_title\"></span>\r\n        <span data-dojo-attach-point=\"closeButtonNode\" class=\"dijitDialogCloseIcon\" data-dojo-attach-event=\"ondijitclick: onCancel\" title=\"${resources.close}\" role=\"button\" tabIndex=\"-1\">\r\n            <span data-dojo-attach-point=\"closeText\" class=\"closeText\" title=\"${resources.cancel}\">x</span>\r\n        </span>\r\n    </div>\r\n    <div data-dojo-attach-point=\"contentContainerNode\" class=\"epi-dialogPaneContent\">\r\n        <span data-dojo-attach-point=\"iconNode\" class=\"dijitIcon dijitInline epi-dialogIcon\"></span>\r\n        <div data-dojo-attach-point=\"headingNode\" class=\"epi-dialogDescriptionHeader\"></div>\r\n        <div data-dojo-attach-point=\"descriptionNode\" class=\"epi-dialogDescriptionSummary\"></div>\r\n        <div data-dojo-attach-point=\"containerNode\" class=\"dijitDialogPaneContentArea\"></div>\r\n        <div data-dojo-attach-point=\"actionContainerNode\" class=\"dijitDialogPaneActionBar\"></div>\r\n    </div>\r\n</div>"}});define("epi/shell/widget/dialog/_DialogBase",["epi","dojo","dojo/_base/array","dojo/dom-class","dojo/dom-style","dojo/query","dojo/text!./templates/Dialog.html","dijit/a11y","dijit/focus","dijit/Dialog","epi/shell/widget/dialog/_DialogMixin","epi/shell/widget/_FocusableMixin"],function(_1,_2,_3,_4,_5,_6,_7,_8,_9,_a,_b,_c){return _2.declare("epi.shell.widget.dialog._DialogBase",[_a,_b,_c],{templateString:_7,heading:"",description:"",dialogClass:"",iconClass:"dijitNoIcon",destroyOnHide:true,postMixInProperties:function(){this.inherited(arguments);this.resources=_1.resources.action;},buildRendering:function(){this.inherited(arguments);if(this.dialogClass){_4.add(this.domNode,this.dialogClass);}this.set("closeIconVisible",this.closeIconVisible);this.set("iconClass",this.iconClass);this.set("heading",this.heading);this.set("description",this.description);},postCreate:function(){this.inherited(arguments);if(this.content&&this.content.executeDialog){this.connect(this.content,"executeDialog","onExecute");}if(this.content&&this.content.cancelDialog){this.connect(this.content,"cancelDialog","onCancel");}},hide:function(){var _d=_2.hitch(this,function(){if(this.destroyOnHide){this.destroyRecursive();}});return this.inherited(arguments).then(_d);},_getFocusItems:function(){var _e=_8._getTabNavigable(this.contentContainerNode);this._firstFocusItem=_e.lowest||_e.first||this.closeButtonNode||this.domNode;this._lastFocusItem=_e.last||_e.highest||this._firstFocusItem;},_setDialogContentNode:function(_f,_10){_f.innerHTML=_10;_5.set(_f,"display",_10?"":"none");var _11=_6("div",this.contentContainerNode);_11.removeClass("epi-firstVisible");_3.some(_11,function(_12){if(_5.get(_12,"display")!=="none"){_4.add(_12,"epi-firstVisible");return true;}});},_setHeadingAttr:function(_13){this._setDialogContentNode(this.headingNode,_13);},_setDescriptionAttr:function(_14){this._setDialogContentNode(this.descriptionNode,_14);},_setIconClassAttr:function(_15){_4.remove(this.iconNode,this.iconClass);this._set("iconClass",_15);_4.add(this.iconNode,this.iconClass);}});});