
function Main_Login() {
	
	var logoPanel = Ext.create("Ext.form.Panel", {
	    baseCls: 'x-plain',
	    id: 'login-logo',
	    region: 'center'
	})
	var loginForm = Ext.create("Ext.form.FormPanel", {
	    region: 'south',
	    border: false,
	    bodyStyle: "padding: 20px;background-color: #F0F0F0 ",
	    //baseCls: 'x-plain',
	    waitMsgTarget: true,
	    labelWidth: 60,
	    defaults: {
	        width: 280
	    },
	    height: 50,
	    items: [{
	        xtype: 'textfield',
	        fieldLabel: 'Email',
	        labelStyle: "text-align:right;font-weight:bold;color:#0072E3",
	        id: 'loginname_id',
	        name: 'loginname',
	        fieldCls: 'yonghuming',
	        blankText: 'Email cannot be empty',
	        validateOnBlur: false,
	        allowBlank: false
	    }
                //, {
                //	xtype : 'textfield',
                //	inputType : 'password',
                //	name : 'pwd',
                //	cls : 'mima',
                //	blankText : 'Password cannot by empty',
                //	fieldLabel : 'Password',
                //	validateOnBlur : false,
                //	allowBlank : false
                //}
	    ]
	});

	var win = Ext.create('Ext.Window', {
        id : 'LoginWin',
		title : 'Supplier Login Window',
		iconCls : 'locked',
		width : 428,
		height : 308,
		resizable : false,
		draggable : true,
		modal : false,
		closable : false,
		layout : 'border',
		//bodyStyle : 'padding:5px;',
		plain: false,
		items : [logoPanel, loginForm],
		buttonAlign : 'center',
		buttons : [{
		    text: 'Login',
		    id: 'loginBtn',
			cls : "x-btn-text-icon",
			icon : "/Content/icons/lock_open.png",
			height : 30,
			listeners: {
			    
			    click: function () {
                    //test@pdh.com
			        if (loginForm.form.isValid()) {
			            var myMask = new Ext.LoadMask(Ext.getCmp('LoginWin'), { msg: "Loging,Please Wait...", removeMask: true });
			            myMask.show();
			            Ext.Ajax.request({
			                        url: 'LoginHandler.aspx/Page_Load',
			                        method: 'POST',
			                        params: loginForm.form.getValues(),
			                        callback: function (options, success, response) {
			                            if (myMask != undefined) { myMask.hide(); }
			                            if (success) {
			                                var result = Ext.decode(response.responseText);
			                                if (result.success) {
			                                    window.location.href = 'MasterPage.aspx';
			                                } else {
			                                    Ext.Msg.alert('Warming', "Email is Error! Please Check Again!");
			                                }

			                            } else {
			                                Ext.Msg.alert('Warming', "Server is Error! Please Check Again!");
			                            }
			                        }
			                    });
			           
			        }

			    }
			}
		}, {
			text : 'Reset',
			cls : "x-btn-text-icon",
			icon : "/Content/icons/arrow_redo.png",
			height : 30,
			handler : function() {
				loginForm.form.reset();
			}

		}],
		listeners: {
		    show: function(win) {
		        Ext.getCmp('loginname_id').focus(false, 200);
		    }
		}
	});
	
	win.show();
};
function SubmitOrHidden (evt) {
    evt = window.event || evt;
    if (evt.keyCode == 13) {//如果取到的键值是回车
        document.getElementById('loginBtn').click();
    }
};
Ext.onReady(function () {
            Ext.getBody().setStyle('background-color', '#E0E0E0');
            Main_Login();
            window.document.onkeydown = SubmitOrHidden;//当有键按下时执行函数
		});