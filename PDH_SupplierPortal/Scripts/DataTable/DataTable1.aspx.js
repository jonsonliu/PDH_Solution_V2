function Data_Table1() {

    // 表格Store
    var gridStore = new Ext.data.Store({
        url: "/DataTable/DataDetail1",
        remoteSort: true,
        reader: new Ext.data.JsonReader({
            root: 'data',
            totalProperty: 'total'
        }, [{
            name: 'SSN',
            type: 'string'
        }, {
            name: 'ORIG_HIRE_DATE',
            type: 'date'
        }, {
            name: 'FIRSTNAME',
            type: 'string'
        }, {
            name: 'MIDDLENAME',
            type: 'string'
        }, {
            name: 'LASTNAME',
            type: 'string'
        }])
    });
    gridStore.setDefaultSort('ORIG_HIRE_DATE', 'DESC');

    
    // 定义表格列
    var sm = new Ext.grid.CheckboxSelectionModel();
    var cm = new Ext.grid.ColumnModel([new Ext.grid.RowNumberer(), sm, {
        header: "代码",
        dataIndex: 'SSN',
        sortable: true,
        xtype: 'textfield'
    },{
        xtype: 'datecolumn',
        format: 'Y年m月d日 H:i:s',
        header: "创建时间",
        width: 150,
        dataIndex: 'ORIG_HIRE_DATE',
        sortable: true
    }, {
        header: "FIRSTNAME",
        dataIndex: 'FIRSTNAME',
        sortable: true,
        xtype: 'textfield'
    }, , {
        header: "MIDDLENAME",
        dataIndex: 'MIDDLENAME',
        sortable: true,
        xtype: 'textfield'
    }, {
        header: "LASTNAME",
        dataIndex: 'LASTNAME',
        sortable: true,
        xtype: 'textfield'
    }]);
    // 分页工具栏
    var bbar = new Ext.PagingToolbar({
        displayInfo: true,
        emptyMsg: "没有数据显示",
        displayMsg: "显示从{0}条数据到{1}条数据，共{2}条数据",
        store: gridStore,
        pageSize: ALL_PAGESIZE_SETTING,
        plugins: [filters]
    })
    // 定义表格
    var grid = new Ext.grid.GridPanel({
        sm: sm,
        cm: cm,
        enableColumnMove: false,
        plugins: [gridEditor, filters],
        stripeRows: true,
        frame: true,
        border: true,
        layout: "fit",
        store: gridStore,
        loadMask: true,
        tbar: tbar,
        bbar: bbar
    });
    grid.getSelectionModel().on('selectionchange', function (sm) {
        grid.delBtn.setDisabled(sm.getCount() < 1);
    });
    JsHelper.ExtTabDoLayout(grid);
    // 注意 在ExtTabDoLayout之后Load
    gridStore.load({
        params: {
            start: 0,
            limit: ALL_PAGESIZE_SETTING
        }
    });
}

Data_Table1();