//>>built
define("epi/shell/command/_WidgetCommandProviderMixin",["dojo/_base/declare","epi/shell/command/_CommandConsumerMixin","epi/shell/command/_GlobalCommandProviderMixin"],function(_1,_2,_3){return _1([_3],{startup:function(){if(this._started){return;}this.inherited(arguments);var _4=this.getConsumer();if(!_4){return;}this._consumer=_4.addProvider(this);this.initializeCommandProviders();},getConsumer:function(){if(this.isInstanceOf(_2)){return this;}var _5=this.getParent();while(_5){if(_5.isInstanceOf(_2)){return _5;}_5=_5.getParent();}},uninitialize:function(){if(this._consumer){this._consumer.removeProvider();}}});});