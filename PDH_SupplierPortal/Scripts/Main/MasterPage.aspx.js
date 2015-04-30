function Main_MasterPage() {

    // 居顶工具栏
    var topBar = new Ext.Toolbar({
        region: 'north',
        border: false,
        cls: 'app-header',
        split: true,
        height: 51,
        minSize: 26,
        maxSize: 26,
        items: [{
            xtype: 'label',
            text: '',
            cls: 'logo-image'
        }, {
            xtype: 'label',
            text: 'Suppler Portal',
            cls: 'logo-text'
        }, "->", "-", {
            xtype: 'button',
            text: sessionName,
            cls: 'x-btn-text-icon',
            icon: '/Content/icons/user.png',
            disabled: true,
            disabledClass: ''
        }, "-", {
            xtype: "button",
            minWidth: 80,
            text: "Logout",
            cls: "x-btn-text-icon",
            icon: "/Content/icons/lock_go.png",
            handler: function (btn, e) {
                Ext.MessageBox.confirm("Message","Are You Sure Logout And Return To The Login Page?", function (btn) {
                    if (btn == 'yes') {
                        Ext.Ajax.request({
                            url: 'LogoutHandler.aspx',
                            success: function (response, opts) {
                                var result = Ext.decode(response.responseText);
                                if (result.success) {
                                    window.location.href = 'LoginPage.aspx';
                                } else {
                                    Ext.Msg.alert(result.msg);
                                }
                            }
                        });
                    }
                });
            }
        }]
    });

    var treeStore = Ext.create('Ext.data.TreeStore', {
        fields: ['id', 'text', 'leaf', 'url', 'iconCls'],
        root: {
            expanded: true,
            children: [
                      {
                          id: "0001",
                          text: "Product Information",
                          leaf: true,
                          url: "",
                          iconCls: "/Content/icons/user.png"
                      },
                      {id:"account", text: "Account Information", leaf: true }
            ]
        }
    });

    var menu = Ext.create('Ext.tree.Panel', {
        title: 'Function Menu',
        region: "west",
        width: 200,
        height: 150,
        store: treeStore,
        autoScroll: true,
        enableTabScroll: true,
        collapsible: true,
        collapsed: false,
        rootVisible: false
    });
    menu.on('itemclick', function (view, record) {
        var leaf = record.get('leaf');
        if (leaf) {
            var tab = tabMain.getComponent('tab_' + record.get('id'));
            if (!tab) {
                //tab = new Ext.Panel({
                //    id: 'tab_' + record.get('id'),
                //    closable: true,
                //    title: node.text,
                //    autoScroll: true,
                //    border: false,
                //    fitToFrame: true,
                //    html: '<iframe id="' + "frame_tab_" + '' + record.get('id') + ' " src="' + record.get('url') + '" frameborder="0" width="100%" height="100%"></iframe>'
                //})
                //tabMain.add(tab);
                //tabMain.setActiveTab(tab);
            } else {
                refreshProductTab();
            }
            
        }
    });
    function refreshProductTab() {
        {
            var tab = tabMain.getComponent('tab_0001');
            tabMain.remove(tab);
            var new_tab = new Ext.Panel({
                id: 'tab_0001',
                title: 'Product Info',
                iconCls: 'house',
                border: false,
                fitToFrame: true
            });
            tabMain.add(new_tab);
            tabMain.setActiveTab(new_tab);
            var localData = [];
            var myMask = new Ext.LoadMask(Ext.getCmp('tab_0001'), { msg: "Loading Data,Please Wait...", removeMask: true });
            myMask.show();
            Ext.Ajax.request({
                url: '/Views/DataTable/ViewCrmProductData.aspx/Page_Load',
                method: 'GET',
                params: {},
                callback: function (options, success, response) {
                    if (myMask != undefined) { myMask.hide(); }
                    if (success) {
                        localData = Ext.decode(response.responseText);
                        Product_Grid(localData, new_tab);
                    } else {
                        Ext.Msg.alert('Warming', "Data Load Error! Please Refresh Page!");
                    }
                }
            });
        }
    }
    // 主显示区
    var tabMain = new Ext.TabPanel({
        id: "Main_MasterPage_TabMain",
        region: "center",
        autoScroll: true,
        enableTabScroll: true,
        activeTab: 0,
        items: [new Ext.Panel({
            id: 'tab_0001',
            title: 'Product Info',
            iconCls: 'house',
            border: false,
            fitToFrame: true
        })]
    });
    // 居底工具栏
    var footBar = new Ext.toolbar.Toolbar({
        region: "south",
        items: ["->", "Copyright All Reserved"]
    });

    // 创建框架
    Ext.create('Ext.container.Viewport', {
        id: "Main_MasterPage_ViewPort",
        layout: 'border',
        items: [tabMain, topBar, footBar, menu]
    });

}

function Product_Grid(oldLocalData, tab) {
    var localData = oldLocalData;
    var exit_store = Ext.getCmp('productStore');
    if (exit_store != null && exit_store != "undefined") {
        exit_store.reload();
        return;
    }
    
    // register model
    Ext.define('Product', {
        extend: 'Ext.data.Model',
        idProperty: 'ProductId',
        fields: [
           { name: 'ProductId' },
           { name: 'ProductName' },
           { name: 'ProductSku' },
           { name: 'Quantity', type: 'float' },
           { name: 'Brand' },
           { name: 'CommissionRate', type: 'float' },
           { name: 'PdhPrice', type: 'float' },
           { name: 'TradePrice', type: 'float' },
           { name: 'RRP', type: 'float' }
           
        ]
    });


    // create the data store
    var store = Ext.create('Ext.data.Store', {
        id:'productStore',
        model: 'Product',
        remoteSort: true,
        pageSize: 15,
        proxy: {
            type: 'pagingmemory',
            data: localData,
            autoLoad: true,
            reader: {
                type: 'json'
            }
        }
    });

    
    // create the Grid
    var myEditing = Ext.create('Ext.grid.plugin.RowEditing', {
        saveBtnText: 'Update', 
        cancelBtnText: "Cancle", 
        autoCancel: false, 
        clicksToMoveEditor: 1, //双击进行修改  1-单击   2-双击    0-可取消双击/单击事件
        autoCancel: false,
        listeners: {
            edit: function (e) {
                var n_productid = e.context.record.get('ProductId');
                var n_quantity = e.context.record.get('Quantity');
                //Ext.Msg.alert(n_productid + '------' + n_quantity);
                if (isNaN(n_quantity)) {
                    Ext.Msg.alert('Warming', "Product Quantity Must Be Number!");
                    return;
                }
                var myMask = new Ext.LoadMask(Ext.getCmp('tab_0001'), { msg: "Update Product,Please Wait...", removeMask: true });
                myMask.show();
                Ext.Ajax.request({
                    url: '/Views/DataTable/UpdateCrmProduct.aspx/Page_Load',
                    method: 'GET',
                    params: {
                        "productid": n_productid,
                        "quantity": n_quantity
                    },
                    callback: function (options, success, response) {
                        if (myMask != undefined) { myMask.hide(); }
                        if (success) {
                            refreshProductList(tab, grid, store);
                        } else {
                            Ext.Msg.alert('Warming', "Update Product Error!");
                        }
                    }
                });
            }
        }
    });
    Ext.override('Ext.grid.RowEditor', {

	    getFloatingButtons: function() {
		    btnsCss = cssPrefix + 'grid-row-editor-buttons'
	    },

	    reposition: function(animateConfig) {

	        getFloatingButtons();

	    }	

});
    var grid = Ext.create('Ext.grid.Panel', {
        id : 'product-grid',
        title: 'Product Grid',
        store: store,
        header: false,
        stripeRows: true,
        border: false,
        bodyBorder: false,
        minHeight:300,
        //height: 400,
        width: '100%',

        frame: true,
        resizable: {
            handles: 's'
        },
        columns: [{
            text: 'Quantity',
            sortable: true,
            menuDisabled : true,
            dataIndex: 'Quantity',
            width: 75,
            editor: {
                xtype: 'numberfield',
                allowBlank: false,
                minValue: 0,
                maxValue: 150000
            }
        }, {
            text: 'Brand',
            sortable: true,
            menuDisabled: true,
            dataIndex: 'Brand',
            width: 75
        }, {
            id: 'productName',
            text: 'ProductName',
            sortable: true,
            menuDisabled: true,
            dataIndex: 'ProductName',
            width: 120,
            flex: 1  //瓜分剩余宽度，按比例分
        }, {
            text: 'ProductSku',
            sortable: true,
            menuDisabled: true,
            dataIndex: 'ProductSku',
            width: 100
        }, {
            text: 'CommissionRate',
            sortable: true,
            menuDisabled: true,
            dataIndex: 'CommissionRate',
            width: 120
        },{
            text: 'PdhPrice',
            sortable: true,
            menuDisabled: true,
            renderer: Ext.util.Format.usMoney,
            dataIndex: 'PdhPrice',
            width: 100
        },{
            text: 'TradePrice',
            sortable: true,
            menuDisabled: true,
            renderer: Ext.util.Format.usMoney,
            dataIndex: 'TradePrice',
            width: 100
        }, {
            text: 'RRP',
            sortable: true,
            menuDisabled: true,
            renderer: Ext.util.Format.usMoney,
            dataIndex: 'RRP',
            width: 100
        }, {
            text: 'ProductId',
            sortable: true,
            menuDisabled: true,
            dataIndex: 'ProductId',
            hidden: true
        }
        //,{
        //    menuDisabled: true,
        //    sortable: false,
        //    menuDisabled: true,
        //    xtype: 'actioncolumn',
        //    width: 50,
        //    items: [{
        //        id:'modify_btn',
        //        iconCls: 'modify',
        //        text:'Modify',
        //        tooltip: 'Modify Product',
        //        handler: function (grid, rowIndex, colIndex) {
        //            var rec = grid.getStore().getAt(rowIndex);
        //            //Ext.Msg.alert('Modify', 'Modify ' + rec.get('ProductSku'));
        //            var productWin = Ext.create('Ext.window.Window', {
        //                animateTarget: 'modify_btn',
        //                layout: 'table',            //内部元素布局方式{absolute accordion anchor border card column fit form table}
        //                width: 500,
        //                height: 200,
        //                closeAction: 'hide',        //窗口关闭的方式：hide/close
        //                plain: true,
        //                title: "Product Modify",
        //                maximizable: true,          //是否可以最大化
        //                minimizable: false,          //是否可以最小化
        //                closable: true,            //是否可以关闭
        //                modal: true,                //是否为模态窗口
        //                resizable: false,           //是否可以改变窗口大小
        //                items: [Ext.create('Ext.form.Panel', {
        //                    width: 500,height: 200,
        //                    items: [  
        //                        {
        //                            xtype: 'textfield',
        //                            name:'productname',
        //                            fieldLabel: 'ProductName',
        //                            width: 450,
        //                            labelWidth: 150,
        //                            labelCls:'win-label',
        //                            fieldCls: 'win-text',
        //                            value: rec.get('ProductName'),
        //                            disabled: true
        //                        }, {
        //                            xtype: "textfield",
        //                            name: 'productsku',
        //                            fieldLabel: 'ProductSku',
        //                            width: 450,
        //                            labelWidth: 150,
        //                            labelCls: 'win-label',
        //                            fieldCls: 'win-text',
        //                            value: rec.get('ProductSku'),
        //                            disabled:true
        //                        }, {
        //                            xtype: "textfield",
        //                            id:'mod-quantity',
        //                            name:'quantity',
        //                            fieldLabel: 'Quantity',
        //                            width: 450,
        //                            labelWidth: 150,
        //                            labelCls: 'win-label',
        //                            fieldCls: 'win-text',
        //                            value: rec.get('Quantity'),
        //                            allowBlank: false,
        //                            vtype: 'alphanum'
        //                        }]
        //                })
        //                    ],
        //                buttons: [{
        //                    text: 'Commit',
        //                    handler: function () {
                               
        //                        var mod_qv = Ext.getCmp('mod-quantity').value;
        //                        if (isNaN(mod_qv)) {
        //                            Ext.Msg.alert('Warming', "Product Quantity Must Be Number!");
        //                            return;
        //                        }
        //                        var myMask = new Ext.LoadMask(productWin, { msg: "Update Product,Please Wait...", removeMask: true });
        //                        myMask.show();
        //                        Ext.Ajax.request({
        //                            url: '/Views/DataTable/UpdateCrmProduct.aspx/Page_Load',
        //                            method: 'GET',
        //                            params: {
        //                                "productid": rec.get('ProductId'),
        //                                "quantity": Ext.getCmp('mod-quantity').value
        //                            },
        //                            callback: function (options, success, response) {
        //                                if (myMask != undefined) { myMask.hide(); }
        //                                if (success) {
        //                                    productWin.close();
        //                                    productWin.destroy();
                                            
        //                                    refreshProductList(tab, grid, store);
        //                                } else {
        //                                    productWin.close();
        //                                    productWin.destroy();
        //                                    Ext.Msg.alert('Warming', "Update Product Error!");
        //                                }
        //                            }
        //                        });
                                
        //                    }
        //                }, {
        //                    text: 'Cancle',
        //                    handler: function () {
        //                        productWin.close();
        //                        productWin.destroy();
        //                    }
        //                }],
        //                listeners: {
        //                    'show': function () {
        //                        Ext.getCmp('mod-quantity').focus(true, true);
        //                    }
        //                }
        //            }).show();
        //        }
        //    }]
        //}

        ],
        selType: 'rowmodel',
        plugins: [myEditing],
        bbar: Ext.create('Ext.PagingToolbar', {
            id : 'pag-toolbar',
            pageSize: 15,
            store: store,
            displayInfo: true
            //plugins: new Ext.ux.ProgressBarPager()
            //plugins: new Ext.ux.SlidingPager()
        })
    });
    var topQueryPanel = Ext.create('Ext.form.Panel', {
        width: '100%',
        heigth: 200,
        border:false,
        bodyBorder: false,
        frame: false,
        layout: 'column',
        items: [
				{
				    columnWidth: .60,
				    layout: 'form',
				    border: false,
				    items: [new Ext.form.TextField({
				        id: 'queryProductName',
				        width: 180,
				        border: false,
				        labelCls: 'win-label',
				        fieldCls: 'win-text',
				        fieldLabel: 'ProductName',
				        maxLength: 22

				    })]
				},
				{
				    columnWidth: .40,
				    layout: 'form',
				    border: false,
				    items: [new Ext.form.TextField({
				        id: 'queryProductSku',
				        width: 180,
				        border: false,
				        labelCls: 'win-label',
				        fieldCls: 'win-text',
				        fieldLabel: 'ProductSku'
				    })]
				
		}],
        buttons: [
				{
					text: 'Filter',
					handler: function () {
						var qpn = Ext.getCmp('queryProductName').value;
						var qps = Ext.getCmp('queryProductSku').value;
						var localData_new = [];
						if (qpn != null || qps != null) {
						    if (qpn != '' || qps != '') {
						        
						        for (var i = 0; i < localData.length; i++) {
						            var prd = localData[i];
						            if (contains(prd.ProductName,qpn,true) || contains(prd.ProductSku,qps,true)) {
						                localData_new.push(prd);
						            }
						        }
						    }
						    else {
						        localData_new = localData;
						    }
						}
						
						store.proxy.data = localData_new;
						store.commitChanges();
						store.load();
					}
				}, {
					text: 'Clean',
					handler: function () {
						topQueryPanel.getForm().reset();
					}
				}
        ]

    });
    var secondMainPanel = new Ext.Panel({
        header: false,
        frame: false,
        border: false,
        bodyBorder: false,
        autoHeight: false,
        autoWidth: false,
        autoScroll: false,
        items: [topQueryPanel, grid]
    });
    tab.add(secondMainPanel);
   // grid.render('tab_0001');
    
    store.load();

    // *
  //* string:原始字符串
  //* substr:子字符串
  //* isIgnoreCase:忽略大小写
  //* 

    function contains(string, substr, isIgnoreCase)
    {
        if (substr == "")
        {
            return false;
        }
        if (isIgnoreCase)
        {
            string = string.toLowerCase();
            substr = substr.toLowerCase();
        }

        var startChar = substr.substring(0, 1);
        var strLen = substr.length;

        for (var j = 0; j<string.length - strLen + 1; j++)
        {
            if (string.charAt(j) == startChar)  //如果匹配起始字符,开始查找
            {
                if (string.substring(j, j+strLen) == substr)  //如果从j开始的字符与str匹配，那ok
                {
                    return true;
                }   
            }
        }
        return false;
    }
    function refreshProductList(tab, grid, store) {
        {
            var myMask = new Ext.LoadMask(Ext.getCmp('tab_0001'), { msg: "Loading Data,Please Wait...", removeMask: true });
            myMask.show();
            Ext.Ajax.request({
                url: '/Views/DataTable/ViewCrmProductData.aspx/Page_Load',
                method: 'GET',
                params: {},
                callback: function (options, success, response) {
                    if (myMask != undefined) { myMask.hide(); }
                    if (success) {
                        localData = Ext.decode(response.responseText);
                        store.proxy.data = localData;
                        store.commitChanges();
                        store.load();
                        

                    } else {
                        Ext.Msg.alert('Warming', "Data Load Error! Please Refresh Page!");
                    }
                }
            });
        }
    }
}
Ext.onReady(function () {
    Main_MasterPage();
    var localData = [];
    var tab = Ext.getCmp('tab_0001');
    var myMask = new Ext.LoadMask(tab, { msg: "Loading Data,Please Wait...", removeMask: true });
    myMask.show();
    Ext.Ajax.request({
        url: '/Views/DataTable/ViewCrmProductData.aspx/Page_Load',
        method: 'GET',
        params: {},
        callback: function (options, success, response) {
            if (myMask != undefined) { myMask.hide(); }
            if (success) {
                localData = Ext.decode(response.responseText);
                Product_Grid(localData, tab);
            } else {
                Ext.Msg.alert('Warming', "Data Load Error! Please Refresh Page!");
            }
        }
    });
			
});