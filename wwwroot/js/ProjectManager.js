
            $(document).ready(function () {

                BindData();
                function BindData() {
                    $('.table td').remove();
                    $.ajax({
                        type: "GET",
                        url: "/api/Manager/",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            $("#DIV").html('');
                            var DIV = '';
                            var actions = $("table td:last-child").html();
                            console.log(response);
                            var data = response.data;
                            $.each(data, function (i, item) {
                                var rows = "<tr>" +
                                    "<td id='Name'>" + item.projectName + "</td>" +
                                    "<td id='Comments'>" + item.comments + "</td>" +
                                    "<td id='ModifiedBy'>" + item.modifiedBy + "</td>" +
                                    "<td id='ModifiedOn'>" + Date(item.modifiedOn, "dd-MM-yyyy") + "</td>" +
                                    "<td id='actions'>" + '<i class="edit fa fa-pencil-square-o" title="Edit" data-toggle="tooltip"></i><i style="margin-left:15%" id="btnDelete" class="delete fa fa-trash-o" title="Delete" data-toggle="tooltip"></i><i style="margin-left:15%" id="download" class="download fa fa-download" title="Download" data-toggle="tooltip"></i>' + "</td>" +
                                    "</tr>";
                                $('#Table').append(rows);
                            }); //End of foreach Loop
                            console.log(data);
                            $(".delete").click(function () {
                                event.preventDefault();
                                var projectName = $(this).closest('tr').find('#Name').text();
                                var param = {
                                    "projectName": projectName
                                }
                                DeleteProject(param);
                            });

                            function DeleteProject(param) {
                                $.ajax({
                                    url: "/api/Manager/",
                                    type: "DELETE",
                                    contentType: "application/json",
                                    data: JSON.stringify(param),
                                    success: function () {
                                        bootbox.alert("deleted successfully.");
                                        BindData();
                                    },
                                    error: function () {
                                    }
                                });
                            }

                            //To download the selected project
                            $(".download").click(function () {
                                event.preventDefault();
                                var projectName = $(this).closest('tr').find('#Name').text();                              

                                //To append project name(which needs to get downloaded) in url
                                var querystring = 'ProjectName=' + projectName;
                                $.ajax({
                                    type: "GET",
                                    url: "/api/Manager/DownloadProject",
                                    contentType: "application/json",
                                    data: querystring,
                                    success: function(response) {
                                        bootbox.alert(response+" downloaded successfully.");
                                    }
                                });
                            });



                            //To edit the exsiting project
                            $(".edit").click(function () {
                                event.preventDefault();
                                var projectName = $(this).closest('tr').find('#Name').text();
                                $.ajax({
                                    type: 'GET',
                                    url: '/Files/ProjectDetails.json',
                                    dataType: 'json',
                                    success: function (response) {
                                        var items = response.data
                                        var createHTML = '<form id="UpdateForm"><div class="form-group"><label for="txtProjectName">Project Name</label> <input type="text" disabled class="form-control" id="txtProjectName" placeholder="Enter Project Name...">  </div>    <div class="form-group"><label for="txtModifiedBy">Modified By</label><input type="text" class="form-control" id="txtModifiedBy" placeholder="Modified By...">    </div>    <div class="form-group">        <label for="txtComment">Comment</label>        <textarea class="form-control" type="text" id="txtComment" placeholder="Enter the Comment here..." />    </div>    <div class="form-group"> <label for="sourcePath">Source Path</label> <input type="text" class="form-control" id="sourcePath" placeholder="Enter Source Path..." required> </div>    <button type="submit" class="btn btn-primary" id="btnEdit">Submit</button></form>';//$(template); //type="file" class="form-control-file"
                                        var $bootBox = bootbox.dialog({
                                            message: createHTML,
                                            title: "Edit Project",
                                            onEscape: true,
                                            draggable: true,
                                            className: "updateStyle"
                                        });

                                        $("#txtProjectName").val(projectName);
                                        if (items.length > 0) {
                                            var filteredItem = _.filter(items, function (i) { return i.ProjectName.toLowerCase() == projectName.toLowerCase() })
                                            $("#txtModifiedBy").val(filteredItem[0].ModifiedBy);
                                            $("#txtComment").val(filteredItem[0].Comments);
                                            $("#sourcePath").val(filteredItem[0].SourcePath);
                                        }

                                        $("#btnEdit").click(function () {
                                            UpdateProject();
                                        });
                                    },
                                    failure: function (response) {
                                        alert(response.responseText);
                                    },
                                    error: function (response) {
                                        alert(response.responseText);
                                    }
                                });

                                function UpdateProject() {
                                    var postData = {
                                        "ProjectName": $("#txtProjectName").val(),
                                        "ModifiedBy": $("#txtModifiedBy").val(),
                                        "SourcePath": $("#sourcePath").val(),
                                        "Comments": $("#txtComment").val()
                                    }
                                    $.ajax({
                                        url: "/api/Manager/",
                                        type: "PUT",
                                        contentType: "application/json",
                                        data: JSON.stringify(postData),
                                        success: function () {
                                            bootbox.alert("updated successfully.");
                                            $('.updateStyle').remove();
                                            $('.modal-backdrop').remove();
                                            BindData();
                                        },
                                        error: function () {
                                        }
                                    });
                                }
                            });

                        }, //End of AJAX Success function

                        failure: function (data) {
                            alert(data.responseText);
                        }, //End of AJAX failure function
                        error: function (data) {
                            alert(data.responseText);
                        } //End of AJAX error function
                    });
                }


                $("#addProject").click(function () {
                    var createHTML = '<form id="addForm"><div class="form-group"><label for="txtProjectName">Project Name</label><input type="text" class="form-control" id="txtProjectName" placeholder="Enter Project Name..." required/></div><div class="form-group"><label for="txtCreatedBy">Author</label><input type="text" class="form-control" id="txtCreatedBy" placeholder="Created By..." required/></div><div class="form-group"><label for="txtComment">Comment</label><textarea class="form-control" type="text" id="txtComment" placeholder="Enter the Comment here..." /></div><div class="form-group"><label for="sourcePath">Source Path</label><input type="text" id="sourcePath" required/></div><button class="btn btn-primary" type="submit" id="btnAdd">Submit</input></form>';
                    var $bootBox = bootbox.dialog({
                        message: createHTML,
                        title: "Create Project",
                        onEscape: true,
                        draggable: true,
                        className: "AddStyle"
                    });

                    $("#btnAdd").click(function () {
                        if ($("#txtProjectName").val() != "" && $("#txtCreatedBy").val() != "" && $("#sourcePath").val() != "") {
                            CreateProject();
                        }
                    })

                    function CreateProject() {
                        var postData = {
                            "ProjectName": $("#txtProjectName").val(),
                            "CreatedBy": $("#txtCreatedBy").val(),
                            "SourcePath": $("#sourcePath").val(),
                            "Comments": $("#txtComment").val(),
                        }
                        $.ajax({
                            url: '/api/Manager/',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            type: 'POST',
                            data: JSON.stringify(postData),
                            success: function () {
                                $('.AddStyle').remove();
                                $('.modal-backdrop').remove();
                                BindData();
                            },
                            failure: function (response) {
                                alert(response.responseText);
                            },
                            error: function (response) {
                                alert(response.responseText);
                            }
                        });
                    }
                });
            });
