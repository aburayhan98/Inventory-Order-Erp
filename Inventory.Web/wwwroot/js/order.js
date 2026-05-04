function updateRow(row) {
  const price = Number(row.find(".unit-price").val()) || 0;
  const quantity = Number(row.find(".quantity").val()) || 0;
  const total = price * quantity;

  row.find(".row-total").val(total.toFixed(2));
  updateGrandTotal();
}

function updateGrandTotal() {
  let grandTotal = 0;

  $(".row-total").each(function () {
    grandTotal += Number($(this).val()) || 0;
  });

  $("#grandTotal").text(grandTotal.toFixed(2));
}

function reindexRows() {
  $("#orderBody tr").each(function (index) {
    const row = $(this);

    row.find(".product-select").attr("name", `Items[${index}].ProductId`);
    row.find(".quantity").attr("name", `Items[${index}].Quantity`);
    row.find(".unit-price").attr("name", `Items[${index}].UnitPrice`);
  });
}

$(document).on("change", ".product-select", function () {
  const row = $(this).closest("tr");
  const price = $(this).find(":selected").data("price") || 0;

  row.find(".unit-price").val(price);
  updateRow(row);
});

$(document).on("input change", ".quantity", function () {
  updateRow($(this).closest("tr"));
});

$("#addRow").on("click", function () {
  const newRow = $("#orderBody tr:first").clone();

  newRow.find(".product-select").val("");
  newRow.find(".unit-price").val("");
  newRow.find(".quantity").val(1);
  newRow.find(".row-total").val("");

  $("#orderBody").append(newRow);

  reindexRows();
  updateGrandTotal();
});

$(document).on("click", ".remove-row", function () {
  if ($("#orderBody tr").length === 1) return;

  $(this).closest("tr").remove();

  reindexRows();
  updateGrandTotal();
});

$("form").on("submit", reindexRows);