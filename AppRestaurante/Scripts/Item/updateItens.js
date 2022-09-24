$('#selectItem').change(function () {

    if ($(this).val() === ' ') {
        $('#formUpdate #ItemName').val('');
        $('#formUpdate #ItemPrice').val('');
        $('#hdIdUpdate').val('');
        return;
    }

    $.ajax({
        url: "Item/GetItemById",
        type: "GET",
        data: {
            id: $(this).val()
        },
        success(result, status, xhr) {
            $('#formUpdate #ItemName').val(result.ItemName);
            $('#formUpdate #ItemPrice').val(parseFloat(result.ItemPrice).toFixed(2).toString().replace(".", ","));
            $('#hdIdUpdate').val(result.ItemId);
            console.log('result' + result.ItemCategory);
            $('#formUpdate #ItemCategory').val(result.ItemCategory);
            
        },
        error(xhr, status, error) {
            console.error(xhr);
            console.error(status);
            console.error(error);
        }
    });
});