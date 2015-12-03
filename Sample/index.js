document.addEventListener("DOMContentLoaded", function(event) {
    document.getElementById("target").innerHTML = LightDesk.version;
    //alert(LightDesk.version);
});

function test() {
    var service = LightDesk.resolve("DiagnosticService");
    if (service.Question("Do you want to see the message"))
    {
        service.Message("You said yes!");
    }
    else
    {
        service.Warning("You declined proposal");
    }

    LightDesk.resolve("WindowService").SetTitle("Changed...");
}
