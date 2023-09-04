// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let keywords = [];
var resultBox =  document.getElementById("result-box");
const inputBox = document.getElementById("searchbar");

function func(){
  
    var query = $("#searchbar").val();
    $.ajax({
            url:'/Home/MovieSearch',
            method:'GET',
            data: {query: query},
            dataType: 'json',
            success: function(data){ 
                console.log(data);
                if(data != null)
                    display(data);
             
                
            },
            error: function(xhr,status,error){
                console.log('Error:', status, xhr);
            }
    
        });
                    
}
function display(res){
    const content = res.map((list) => {
        return "<li>" + list + "</li>";
    });
    

    (document).getElementById("result-box").innerHTML = "<ul>" + content.join('') + "</ul>";
}
