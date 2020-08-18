var noteid = -1;
var modalCommentBodyId = "#modal_comment_body";

$(function () {
    //modal gösterildikten sonra modal içerisinde yorumlar içeriği gelecek.
    $('#modal_comment').on('show.bs.modal', function (e) {
        debugger;

        var btn = $(e.relatedTarget); //buton açıldığı andaki buton özellklerini alıyor.
        noteid = btn.data("note-id");

        $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
    });
});

function doComment(btn, e, commentId, spanId) {
    var button = $(btn);
    var mode = button.data("edit-mode");

    if (e === "edit") {
        if (!mode) {
            button.data("edit-mode", true);
            button.removeClass("btn-warning");
            button.addClass("btn-success");
            var btnSpan = button.find("span");
            btnSpan.removeClass("glyphicon-edit");
            btnSpan.addClass("glyphicon-ok");

            $(spanId).addClass("editable");
            $(spanId).attr("contenteditable", true);
            $(spanId).focus();
        }
        else {
            debugger;
            button.data("edit-mode", false);
            button.removeClass("btn-success");
            button.addClass("btn-warning");
            var btnSpan = button.find("span");
            btnSpan.removeClass("glyphicon-ok");
            btnSpan.addClass("glyphicon-edit");

            $(spanId).removeClass("editable");
            $(spanId).attr("contenteditable", false);

            var txt = $(spanId).text();

            $.ajax({
                method: "POST",
                url: "/Comment/Edit/" + commentId,
                data: { text: txt },
            }).done(function (data) {
                if (data.result) {
                    //yorumlar partial tekrar yükle.
                    $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
                } else {
                    alert("Yorum güncellenemedi.");
                }
            }).fail(function () {
                alert("Sunucu ile bağlantı kurulamadı.");
            });
        }
    }
    else if (e === "delete") {
        var dialog_res = confirm("Yorum silinsin mi?");
        if (!dialog_res) return false;

        $.ajax({
            method: "GET",
            url: "/Comment/Delete/" + commentId,
        }).done(function (data) {
            debugger;
            if (data.result) {
                //yorumlar partial tekrar yükle.
                $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
            }
            else {
                alert("Yorum Silinemedi.");
            }
        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı.");
        });
    }
    else if (e === "new_comment") {
        debugger;
        var txt = $("#new_comment_text").val();

        $.ajax({
            method: "POST",
            url: "/Comment/Create/",
            data: { "text": txt, "noteid": noteid }
        }).done(function (data) {
            if (data.result) {
                $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
            }
            else {
                alert("Yorum Eklenemedi.");
            }
        }).fail(function () {
            alert("Sunucu yüklenemedi.");
        });
    }
}