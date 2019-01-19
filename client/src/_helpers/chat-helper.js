
import * as signalr from '@aspnet/signalr';


let connection = null;

export const chatHelper = {

    start,
    stop,
    send,
    sendContactRequest,
    sendToUser
};


function start(options){

    let chaturl = "http://localhost:4000";

    if(options && options.baseurl){
        baseurl = options.baseurl;
    }

    console.log('login token: ', options.token);

    let url = baseurl + "/chat";

    connection = new signalr.HubConnectionBuilder()
    .withUrl(url, { accessTokenFactory: () => options.token})
    .build();

    connection.start().then(err => {

        console.log('connection started**************')
        if(err){

            console.error(err.toString());
        }
        else{
            //connection.invoke('InformContactsOfStatus', true);
        }
    });



    connection.on("ContactRequest", (name) => {

        console.log('received a contact request from ' + name)
        if(options.contactRequest){
            options.contactRequest(name);
        }
    })

    connection.on("ContactRequestAccepted", (name) => {

        console.log(name + ' accepted your contact request')
        if(options.acceptContactRequest){
            options.acceptContactRequest(name);
        }
    })

    connection.on("Send", (name, message) => {
        
        if(options.afterSend){
            options.afterSend(name, message);
        }
    })

    connection.on("SendToUser", (message) => {
        
        console.log('recieved in sendtouser', message);

        if(options.afterSendToUser){
            options.afterSendToUser(message);
        }
    })

    connection.on("ContactStatusUpdate", (userid, status) => {

        if(options.contactStatusUpdate){

            options.contactStatusUpdate(userid, status);
        }
    })
    


}

function stop(){

    if(connection){

        return connection.stop().then(() =>{
            connection = null;
        });
    }
    else{
        return Promise.resolve(true);
    }

}

function send(name, message){

    console.log('chat helper name:', name)
    console.log('chat helper message:', message)
    
    if(!connection){
        throw new Error("No connection found! Please start the chat service.");
    }

    return connection.invoke('Send', message);

}


function sendToUser(message){

    console.log('send to user: ',  message);
    
    return connection.invoke('SendToUser', message);
}


function sendContactRequest(userid){
    console.log('sending contact request to user id ' + userid);
    return connection.invoke('ContactRequest', userid);
}
