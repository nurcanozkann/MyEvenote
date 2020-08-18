$(function () {
    //modal gösterildikten sonra modal içerisinde yorumlar içeriği gelecek.
    $('#modal_notedetail').on('show.bs.modal', function (e) {
        debugger;

        var btn = $(e.relatedTarget); //buton açıldığı andaki buton özellklerini alıyor.
        noteid = btn.data("note-id");

        $("#modal_notedetail_body").load("/Note/GetNoteText/" + noteid);
    });
});
