import config from 'config';
import { authHeader } from '../_helpers';

import { logout } from './user.service'; 

export const contactService = {
    searchByName,
    sendContactRequest,
    acceptContactRequest,
    getReceivedRequests,
    getMyContacts,
    getGuid,
    getConversationForContact
};



function searchByName(name) {
    const requestOptions = {
        method: 'GET',
        headers: authHeader()
    };

    if(!name){

        return new Promise(function(resolve, reject){
            resolve([]);
        });
    }
    else{

        return fetch(`${config.apiUrl}/contact/search/${name}`, requestOptions).then(handleResponse);
    }

}


function getReceivedRequests() {
    const requestOptions = {
        method: 'GET',
        headers: authHeader()
    };


    return fetch(`${config.apiUrl}/contact/receivedrequests`, requestOptions).then(handleResponse);
    
}

function getMyContacts() {

    const requestOptions = {
        method: 'GET',
        headers: authHeader()
    };

    return fetch(`${config.apiUrl}/contact/mycontacts`, requestOptions).then(handleResponse);
    
}

function getGuid(){
    const requestOptions = {
        method: 'GET',
        headers: authHeader()
    };

    return fetch(`${config.apiUrl}/contact/getguid`, requestOptions).then(handleResponse);
}


function sendContactRequest(userid){

    let head = authHeader();
    head['Content-Type'] = "application/json; charset=utf-8";

    var data = { id: userid};

    const requestOptions = {
        method: 'POST',
        headers: head,
        body: JSON.stringify(userid)
    };

    return fetch(`${config.apiUrl}/contact/sendrequest`, requestOptions).then(handleResponse);
    
}

function getConversation(conversationId){

    let head = authHeader();
    head['Content-Type'] = "application/json; charset=utf-8";


    const requestOptions = {
        method: 'POST',
        headers: head,
        body: JSON.stringify(conversationId)
    };

    return fetch(`${config.apiUrl}/contact/getconversation`, requestOptions).then(handleResponse);
    
}

function getConversationForContact(contact){
    let head = authHeader();
    head['Content-Type'] = "application/json; charset=utf-8";


    const requestOptions = {
        method: 'POST',
        headers: head,
        body: JSON.stringify(contact.id)
    };

    return fetch(`${config.apiUrl}/contact/getconversationforcontact`, requestOptions).then(handleResponse);
    
}


function acceptContactRequest(req){

    let head = authHeader();
    head['Content-Type'] = "application/json; charset=utf-8";


    const requestOptions = {
        method: 'POST',
        headers: head,
        body: JSON.stringify(req)
    };

    return fetch(`${config.apiUrl}/contact/acceptrequest`, requestOptions).then(handleResponse);
    
}

function handleResponse(response) {
    return response.text().then(text => {
        const data = text && JSON.parse(text);
        if (!response.ok) {
            if (response.status === 401) {
                // auto logout if 401 response returned from api
                logout();
                location.reload(true);
            }

            const error = (data && data.message) || response.statusText;
            return Promise.reject(error);
        }

        return data;
    });
}