function Main_Index() {
	var panel = new Ext.Panel({
	            layout: 'fit',
	            bodyCls: 'bgimage'
			});
	JsHelper.ExtTabDoLayout(panel);// 注意这里把panel组件加到当前的tabpanel里
}
Main_Index();// 执行方法
