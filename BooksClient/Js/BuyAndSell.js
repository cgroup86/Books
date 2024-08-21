const apiStart = 'https://proj.ruppin.ac.il/cgroup86/test2/tar1/api';

$(document).ready(function () {

    getFromServer();
});

function getFromServer() {


    const user = JSON.parse(sessionStorage.getItem('userData'));
    const userId = user.userId;
    const api = `https://proj.ruppin.ac.il/cgroup86/test2/tar1/api/PersonalLibraries/GetRequestedBooksByBuyer/userId/${userId}`;
    //const api = `${apiStart}/PersonalLibraries/GetRequestedBooksByBuyer/userId/0`;
    ajaxCall("PUT", api, "", function (requestedBooks) {
        //console.log(requestedBooks);
        renderBooksTable('#requestedTable', requestedBooks, "RequestedBooks");
    }, getRequestBooksECB);

}

function getRequestBooksECB(err) {
    console.log(err);
}
function renderBooksTable(tableId, tableData, title) {
    try {
        if (!$.fn.DataTable.isDataTable(tableId)) {
            $(tableId).DataTable({
                data: tableData,
                columns: [
                    {
                        data: "title",
                        title: "Title"
                    },
                    {
                        data: "buyerName",
                        title: "Buyer Name"
                    },
                    {
                        data: null,
                        defaultContent: '<button class="btn acceptBookBtn">Accept</button>' + '<button class="btn rejectBookBtn">Reject</button>',
                        title: "Accept Books",
                        orderable: false
                    }
                    
                ],
            });
            $(tableId).on('click', '.acceptBookBtn', function () {
                const user = JSON.parse(sessionStorage.getItem('userData'));
                const userId = user.userId;
                var table = $(tableId).DataTable();
                var row = $(this).closest('tr');
                var rowData = table.row(row).data();

                var sellerId = userId;
                var buyerId = rowData.buyerId;
                var bookId = rowData.id;

                $.ajax({
                    url: `https://proj.ruppin.ac.il/cgroup86/test2/tar1/api/PersonalLibraries/AcceptRequestToBuy/sellerId/${sellerId}/buyerId/${buyerId}/bookId/${bookId}`,
                    type: 'PUT',
                    success: function (response) {
                        alert('Request Accepted Successfully!');
                        table.row(row).remove().draw();
                    },
                    error: function (error) {
                        console.error('Error accepting request:', error);
                        alert('Failed to accept request.');
                    }
                });
            });

            $(tableId).on('click', '.rejectBookBtn', function () {
                const user = JSON.parse(sessionStorage.getItem('userData'));
                const userId = user.userId;
                var table = $(tableId).DataTable();
                var row = $(this).closest('tr'); 
                var rowData = table.row(row).data();

                var sellerId = userId;
                var buyerId = rowData.buyerId;
                var bookId = rowData.id;

                $.ajax({
                    url: `https://proj.ruppin.ac.il/cgroup86/test2/tar1/api/PersonalLibraries/RejectRequestToBuyProcedure/sellerId/${sellerId}/buyerId/${buyerId}/bookId/${bookId}`,
                    type: 'PUT',
                    success: function (response) {
                        alert('Request Rejected Successfully!');
                        table.row(row).remove().draw(); 
                    },
                    error: function (error) {
                        console.error('Error rejecting request:', error);
                        alert('Failed to reject request.');
                    }
                });
            });


        } else {
            $(tableId).DataTable().clear().rows.add(tableData).draw();
        }
    } catch (err) {
        alert(err);
    }
}
$(document).on('click', 'button.acceptBookBtn', function () {
    var table = $('#requestedTable').DataTable();
    var rowData = table.row($(this).parents('tr')).data();
    (rowData.id, function (bookCount) {
    });
});

function acceptRequestToBy(sellerId,buyerID,bookId) {
    let api = `${apiStart}/Books/getNumBooksInLibraries/${bookId}`;
    ajaxCall("GET", api, "", function (status) {
        callback(status);
    }, function (err) {
        console.log(err);
        callback(0);
    });
}