//>>built
define("epi/shell/widget/Tooltip",["dojo/_base/declare","dojo/string","dojox/html/entities","dojox/html/ellipsis","dijit/Tooltip"],function(_1,_2,_3,_4,_5){return _1("epi.shell.widget.Tooltip",_5,{showDelay:600,tooltipRows:null,tooltipRowTemplate:"<span>${label}</span>: ${text}",tooltipRowNoTextTemplate:"<span>${label}</span>",postCreate:function(){if(this.tooltipRows){this.label="<ul class='dijitInline dijitReset epi-tooltip-ellipsis'>";for(var i=0;i<this.tooltipRows.length;i++){var _6=this.tooltipRows[i];if(!_6.text){_6.text="";}if(_6.htmlEncode===undefined||_6.htmlEncode===true){_6.label=_3.encode(_6.label);_6.text=_3.encode(_6.text);}if(!_6.text){this.label+="<li class='dojoxEllipsis'>"+_2.substitute(this.tooltipRowNoTextTemplate,_6)+"</li>";}else{this.label+="<li class='dojoxEllipsis'>"+_2.substitute(this.tooltipRowTemplate,_6)+"</li>";}}this.label+="</ul>";}this.inherited(arguments);}});});