var LightDesk = {
    version: "0.1.0",
    
    message: function(text){
        native function message(msg);
        return message(text);
    },
        
    resolve: function(service) {
        native function resolve(service);
        return resolve(service);
    },

};
