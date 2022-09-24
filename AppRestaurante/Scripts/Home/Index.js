//#region "Status from table
/**
 * Status from table
 * */
const orderStatus = {
    Empty: 0,
    Reserved: 1,
    Attendance: 2,
    Finished: 3,
}
//#endregion

//#region "Initialize page
$(document).ready(function () {
    $("#Item").change(GetItemUnitPrice);
    $('input[type=text]').change(CalculateSubTotal);
    $('input[type=text]').keyup(CalculateBalance);
    $('#btnAddToList').click(AddToTheItemList);
    $('#btnPayment').click(FinalPayment);
    $('input[name="rdCategoria"]').change(ReloadItemCategory);
    $("#Table").change(GetOrderTable);
})
//#endregion

//#region "Get Order table"
/**
 * Get Order from table
 */
function GetOrderTable() {
    ResetItem();

    let table = $("#Table").val();

    if (table.trim() == '')
        return;

    $.ajax({
        async: true,
        type: 'GET',
        dataType: 'JSON',
        contentType: 'application/json; charset=utf-8',
        data: { "table": table },
        url: '/Home/GetOrderDetails',
        success: function (data) {

            $('#tblReaturantItemList tbody').empty();

            if (data === null || Object.keys(data.result).length === 0 || data.result.Status == orderStatus.Empty) {
                clearTable();
                return;
            }

            if (data.result.Status == orderStatus.Reserved)
                $("#btnCriarReserva").css("visibility", 'hidden');

            $("#lblOrder").html(data.result.Id);
            $('#txtFinalTotal').val(data.result.Total == null ? '0,00' : parseFloat(data.result.Total).toFixed(2));
            $("#txtResponsavel").val(data.result.Responsavel);
            $("#btnCriarReserva").css("visibility", 'hidden');
            $("#btnCancelarReserva").css("visibility", 'hidden');

            GetItensTable();
        },
        error: function () {
            alert('Erro ao obter pedidos da mesa!');
        }
    });
}
//#endregion

//#region "Get Itens from table"
/**
 * Get itens selled from table
 * */
function GetItensTable() {
    let pedido = $("#lblOrder").html();
    ResetItem();

    $.ajax({
        async: true,
        type: 'GET',
        data: { "orderId": pedido },
        url: '/Home/GetItensOfOrder',
        success: function (data) {

            $('#tblReaturantItemList tbody').empty();

            $.each(data.result, function (index, value) {
                debugger;
                let itemList =
                    "<tr>" +
                    "<td hidden>" + value.OrderDetailId + "</td>" +
                    "<td>" + value.ItemName + "</td>" +
                    "<td>" + formatMoney(value.UnitPrice, ".", ",") + "</td>" +
                    "<td>" + formatMoney(value.Quantity, ".", ",") + "</td>" +
                    "<td>" + formatMoney(value.Discount, ".", ",") + "</td>" +
                    "<td>" + formatMoney(value.Total, ".", ",") + "</td>" +
                    "<td> <input type='button' value='Remover' name='remove' class='btn btn-sm btn-danger' onclick='RemoveItem(this)' /></td>" +
                "</tr>";

                $('#tblReaturantItemList tbody').append(itemList);
                FinalItemTotal();
            });
        },
        error: function (requestObject, error, errorThrown) {
            console.error(error);
            console.error(errorThrown);
            alert('Erro ao obter lista de produtos da categoria informada!');
        }
    })
}

/**
 * Format number to money
 * param number
 * param decPlaces
 * param decSep
 * param thouSep
 */
function formatMoney(number, decPlaces, decSep, thouSep) {
    decPlaces = isNaN(decPlaces = Math.abs(decPlaces)) ? 2 : decPlaces,
        decSep = typeof decSep === "undefined" ? "." : decSep;
    thouSep = typeof thouSep === "undefined" ? "," : thouSep;
    var sign = number < 0 ? "-" : "";
    var i = String(parseInt(number = Math.abs(Number(number) || 0).toFixed(decPlaces)));
    var j = (j = i.length) > 3 ? j % 3 : 0;

    return sign +
        (j ? i.substr(0, j) + thouSep : "") +
        i.substr(j).replace(/(\decSep{3})(?=\decSep)/g, "$1" + thouSep) +
        (decPlaces ? decSep + Math.abs(number - i).toFixed(decPlaces).slice(2) : "");
}

//document.getElementById("b").addEventListener("click", event => {
//    document.getElementById("x").innerText = "Result was: " + formatMoney(document.getElementById("d").value);
//});
//#endregion

//#region "Create reserve"
/**
 * Create Reservation in table
 * */
function createReservation() {
    let table = $("#Table").val();

    if (table.trim() == '')
        return;

    let responsavel = $("#txtResponsavel").val();

    if (responsavel.trim() == '') {
        alert("Informe um responsavel para a reserva!")
        return;
    }

    $.ajax({
        type: 'POST',
        data: { "table": parseInt(table), "responsable": responsavel },
        url: '/Home/CreateReserve',
        success: function (data) {
            console.table(data);
        },
        error: function (requestObject, error, errorThrown) {
            console.error(error);
            console.error(errorThrown);
            alert('Erro ao fazer reserva!');
        }
    })
}
//#endregion

//#region "Cancel Reservation"
/**
 * Cancel Reservation
 * */
function cancelReservation() {
    let table = $("#Table").val();

    if (table.trim() == '')
        return;

    $.ajax({
        async: true,
        type: 'POST',
        data: { "table": parseInt(table) },
        url: '/Home/DeleteReserve',
        success: function (data) {
            alert(data.message);
        },
        error: function () {
            alert('Erro ao cancelar reserva!');
        }
    })
}
//#endregion

//#region "Clear table
/**
 * Clear data from table
 * */
function clearTable() {
    $("#txtResponsavel").val('');
    $('#txtFinalTotal').val('0,00');
    $("#btnCriarReserva").css("visibility", 'visible');
    $("#btnCancelarReserva").css("visibility", 'visible');
}
//#endregion

//#region "Reload itens by category"
/**
 * Reload item by category
 * 
 */
function ReloadItemCategory() {
    let category = $("#rdCategoria:checked").val();

    $.ajax({
        async: true,
        type: 'GET',
        dataType: 'JSON',
        contentType: 'application/json; charset=utf-8',
        data: { "category": category.toString() },
        url: '/Home/GetItemByCategory',
        success: function (data) {
            $("#Item").empty();

            let itens = '';
            $.each(data, function (key, value) {
                itens += `<option value=${value.Value} ${value.Selected ? 'selected' : ''}>${value.Text}</option>`;
            });

            $("#Item").append(itens);
        },
        error: function () {
            alert('Erro ao obter lista de produtos da categoria informada!');
        }
    })
}
//#endregion

//#region "Get item unit price"
/**
 * Get unit price from item
 */
function GetItemUnitPrice() {
    var itemId = $("#Item").val();

    if (itemId === '')
        return;

    $.ajax({
        async: true,
        type: 'GET',
        dataType: 'JSON',
        contentType: 'application/json; charset=utf-8',
        data: { itemId: itemId },
        url: '/home/GetItemUnitPrice',
        success: function (data) {
            $('#txtUnitPrice').val(parseFloat(data.data).toFixed(2).replace('.', ','));
            $('#txtQuantity').val('1');
            $('#txtDiscount').val('0');
            CalculateSubTotal();
            FinalItemTotal();
        },
        error: function () {
            alert("Erro ao obter preço unitario do produto");
        }
    })
}
//#endregion

//#region "Reset item"
/**
 * Reset item
 */
function ResetItem() {
    $('#txtUnitPrice').val('');
    $('#txtQuantity').val('');
    $('#txtDiscount').val('');
    $('#txtTotal').val('')
    $('#Item').val(null);
}
//#endregion

//#region "Calculate subtotal
/**
 * Calculate subtotal from item selled
 */
function CalculateSubTotal() {
    var unitPrice = $('#txtUnitPrice').val().replace(',', '.');
    var quantity = $('#txtQuantity').val().replace(',', '.');
    var discount = $('#txtDiscount').val().replace(',', '.');

    var total = (unitPrice * quantity) - discount;
    $('#txtTotal').val(parseFloat(total).toFixed(2).replace('.', ','));
}
//#endregion

//#region "Add item in list"
/**
 * Insert item in list
 */
function AddToTheItemList() {
    let table = $("#Table").val();

    if (table.trim() == '') {
        alert("Informe uma mesa!");
        return;
    }

    if ($("#txtResponsavel").val().trim() === '') {
        alert("Informe um responsavel para a mesa!");
        return;
    }

    if ($("#Item").val().trim() === '') {
        alert("Informe um item para adicionar a lista!");
        return;
    }

    let pedido = $("#lblOrder").html();

    let itemPedido = {
        OrderId: pedido,
        UnitPrice: parseFloat($('#txtUnitPrice').val()),
        Quantity: parseFloat($('#txtQuantity').val()),
        Discount: parseFloat($('#txtDiscount').val()),
        ItemId: $('#Item').val(),
        Total: parseFloat(($('#txtUnitPrice').val().replace(',', '.') * $('#txtQuantity').val()) - $('#txtDiscount').val()),
        TableNumber: table,
        CustomerName: $("#txtResponsavel").val()
    };

    let erro = true;

    $.ajax({
        async: false,
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ orderDetail: itemPedido }),
        url: '/Home/AddItemInOrder',
        success: function (data) {
            erro = false;
        },
        error: function () {
            alert('Erro ao inserir itens na lista!');
        }
    })

    if (erro)
        return;

    GetItensTable();

}
//#endregion

//#region "Calcule final total"
/**
 * Caculate final value for customer
 * */
function FinalItemTotal() {
    $('#txtFinalTotal').val('0.00');
    var FinalTotal = 0.00;
    $('#tblReaturantItemList').find('tr:gt(0)').each(function () {
        var total = parseFloat($(this).find('td:eq(5)').text().replace(',', '.'));
        FinalTotal += total;
    });

    $('#txtFinalTotal').val(parseFloat(FinalTotal).toFixed(2).replace('.', ','));
    $('#txtPaymentTotal').val(parseFloat(FinalTotal).toFixed(2).replace('.', ','));
    $('#txtBalance').val(parseFloat(FinalTotal).toFixed(2).replace('.', ','));
}
//#endregion

//#region "Remove Item"
/**
 * Remove item from db and reload table
 * param itemId
 */
function RemoveItem(itemId) {
    let erro = true;

    $.ajax({
        async: false,
        type: 'DELETE',
        dataType: 'JSON',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({

            "orderId": $("#lblOrder").html(),
            "orderDetailId": $(itemId).closest('tr').children('td')[0].textContent,
        }),
        url: '/Home/RemoveItemFromOrder',
        success: function (data) {
            erro = false;
        },
        error: function () {
            alert('Erro ao inserir itens na lista!');
        }
    })

    if (erro)
        return;

    GetItensTable();
}
    //#endregion

//#region "Calcula balanço"
/**
 * Calcula balanço
 * */
function CalculateBalance() {
    var finalAmount = $('#txtPaymentTotal').val().replace(',', '.');
    var paymentAmount = $('#txtPaymentAmount').val().replace(',', '.');
    var returnAmount = $('#txtReturnTotal').val().replace(',', '.');
    var balanceAmount = parseFloat(finalAmount) - parseFloat(paymentAmount) + parseFloat(returnAmount);
    $('#txtBalance').val(parseFloat(balanceAmount).toFixed(2).replace('.', ','));

    if (parseFloat(balanceAmount) == 0) {
        $('#txtBalance').removeAttr('disabled');
    } else {
        $('#txtBalance').attr('disabled', 'disabled');
    }
}
//#endregion

//#region "Final Payment"
/**
 * final payment
 * */
function FinalPayment() {
    let pedido = $("#lblOrder").html();

    if ($('#Pagamento').val() === '') {
        alert("Informe o tipo de pagamento");
        return;
    }

    $.ajax({
        async: true,
        type: 'POST',
        data: {
            "orderId": pedido,
            "paymentTypeId": $('#Pagamento').val(),
            "payment": $('#txtPaymentAmount').val()
        },
        url: '/Home/FinalPayment',
        success: function (data) {
            alert(data.message);
            $('#closeModal').click();
            GetOrderTable();
        },
        error: function () {
            alert('Erro ao realizar pagamento!');
        }
    })
}
//#endregion
