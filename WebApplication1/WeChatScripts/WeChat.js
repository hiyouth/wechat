
function connection() {
    var corpid = $("#corpID").val();
    var secret = $("#secret").val();
    //下面的连接以后需要考虑传输的安全性
    $.get("/wechat/verifyCorp?corpid=" + corpid + "&corpsecret=" + secret, verifyCorpId);
}

function verifyCorpId(data, status) {
    var a = 1;
}

function syncMem()
{
    //这里teamId=1，是随意传的，正确的是应该在这里传入team的标示
    $.get("/wechat/SyncMember?teamId=1", syncCallBack());
}

function syncCallBack(data,status) {
    var a = 1;
}