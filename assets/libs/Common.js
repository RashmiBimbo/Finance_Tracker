//$(document).ready(function ()
//{
//    // Select all dropdown lists by their element type
//    $("select").change(function ()
//    {
//        console.log("reached");
//        // Call the UpdateToolTip function when any dropdown changes
//        UpdateToolTip();
//        console.log("reach success");
//    });
//});

$(document).ready(function ()
{
    // Select all dropdown lists and attach the UpdateTooltip function to their onchange event
    $("select").change(function ()
    {
        console.log("reached");
        // Call the UpdateToolTip function when any dropdown changes
        UpdateToolTip(this);
        console.log("reach success");   
    });
});

function UpdateToolTip (ddl)
{
    console.log("reached");
    if (ddl.selectedIndex !== -1)
    {
        ddl.title = ddl.options[ddl.selectedIndex].text;
    }
    console.log("reach success");
}