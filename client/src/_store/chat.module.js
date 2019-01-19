
import { chatHelper } from '../_helpers'
import { contactService } from '../_services';
import { ContactMessage, Conversation } from '../_dto'
import _ from 'lodash'
import Toasted from 'vue-toasted'
import Vue from 'vue'

Vue.use(Toasted, { duration: 1500 });


const state = {

    name: null,
    user: null,
    group: null,
    chatting: false,
    searchUsersResults: [],
    receivedRequests: [],
    myContacts: [],
    activeContact: null,
    conversations: [],
    currentConversation: {}

};


const actions = {

    setCurrentUser(context, user){
        context.commit('setUser', user);
    },

    start(context, user){

        // SET UP THE CHAT CONNECTION
        if(!context.state.chatting){

            console.log('start token: ', user.token);
        
            context.commit('setName', user.fullName);
            context.commit('setUser', user);
            context.commit('updateChatting', true);

            const options = {

                token: user.token,

                afterSend: (name, message) => {
                    let line = name + ": " + message;
                    context.commit('addLine', line);
                },

                contactRequest: (name) => {

                    console.log(name + ' sent you a contact request');
                    Vue.toasted.show(name + ' has sent you a contact request')
                    context.dispatch('getReceivedRequests');
                },

                acceptContactRequest: (name) => {
                    
                    context.dispatch('getMyContacts')
                    Vue.toasted.show(name + ' has accepted your contact request')
                },

                afterSendToUser: (message) => {
                    console.log('message: ', message);

                    // UPDATE THE RIGHT CONVERSATION
                    context.dispatch('addMessageToConversation', message);
                },

                contactStatusUpdate: (userid, status) =>{

                    context.dispatch('updateContactStatus', { userid: userid, status: status })
                    
                }
            };

            return chatHelper.start(options);
        }
        
    },

    stop(context){
        return chatHelper.stop().then(() => {
            context.commit('updateChatting', false);
        });
    },

    send(context, message){

        // OLD METHOD TO SEND MESSAGE TO ALL ONLINE USERS
        return chatHelper.send(context.state.name, message);
    },

    sendToUser(context, messageDetails){

        console.log(context.state.conversation)
        // SEND MESSAGE TO SPECIFIC USER
        const st = context.state;

        let mess = new ContactMessage();
        mess.id = 0;
        mess.conversationId = messageDetails.conversationId;
        mess.toId = messageDetails.toId;
        mess.toName = messageDetails.toName;
        mess.message = messageDetails.message;
        mess.fromId = messageDetails.fromId;
        mess.fromName = messageDetails.fromName;
        mess.sentOn = new Date();

        if(!mess.conversationId){
            mess.conversationId = context.getters.currentConversation.id;
        }

        console.log('sending mesage:');
        console.info(mess);
        // var message = new Message();
        return chatHelper.sendToUser(mess).then((err) => {

            //UPDATE THE CONVERSATION
            // let convs = st.conversations;
            // console.log('my convs: ', convs)

            // let x = _.find(convs, x => { return x.id === mess.conversationId })
            // x.messages.push(mess);

            // context.commit('setConversations', convs);

            // console.log(st.conversations)

            // if(err){
            //     console.log('error sending message: ', err)
            // };
            context.dispatch('addMessageToConversation', mess)

        })
    },

    addMessageToConversation(context, message){
            let convs = context.state.conversations;
            console.log('my convs: ', convs)

            let x = _.find(convs, x => { return x.id === message.conversationId })

            if(!x){
                // get from server
                context.dispatch('getConversationForContact', {id: message.fromId }).then(conversation => {
                    console.log('got conversation from server: ', conversation)
                    
                    // addd message

                    //conversation.messages.push(message);

                    convs.push(conversation);
                    context.commit('setConversations', convs);
                })   
            }
            else{
                x.messages.push(message);

                context.commit('setConversations', convs);
            }
            
    },

    sendContactRequest(context, userid){

        // SEND REQUEST TO ADD SOMEONE AS A CONTACT
        console.log('sending contact request to ' + userid)
        //return chatHelper.sendContactRequest(userid);

        return contactService.sendContactRequest(userid);
    },

    findContacts(context, name){

        // SEARCH FOR AN EXISTING USER
        console.log('searching users named: ' + name);

        contactService.searchByName(name).then(results => context.commit('setSearchUserResults', results))
    },

    getGuid(context){

        // GET A GUID FROM THE SERVER
        return contactService.getGuid();
    },

    clearFindContactResults(context){
        context.commit('setSearchUserResults', []);
    },

    getReceivedRequests(context){

        // GET CONTACT REQUESTS PEOPLE HAVE SENT TO ME
        console.log('getting received requests')
        return contactService.getReceivedRequests().then(results => {
            context.commit('setReceivedRequests', results);
        })
    },

    getMyContacts(context){

        // GET MY EXISTING CONTACTS
        contactService.getMyContacts().then(results =>{
            context.commit('setMyContacts', results);

            if(!context.state.activeContact){
                if(results.length > 0){
                    context.dispatch('setContactAsActive', results[0])
                }
            }
        })
    },

    updateContactStatus(context, data){

        console.log('update contact status: ', data)
        let cons = context.state.myContacts;

        let co = _.find(cons, function(x){
            return x.id == data.userid;
        })

        if(co){
            co.online = data.status;

            if(data.status){
                Vue.toasted.show(co.fullName + ' is online')
            }
            else{
                //Vue.toasted.show(co.fullName + ' has left')
            }
        }

        context.commit('setMyContacts', cons);
    },

    acceptContactRequest(context, request){

        return contactService.acceptContactRequest(request).then(() =>{

            return context.dispatch('getReceivedRequests');
        })
    },

    

    setCurrentConversationToContact(context, contact){

         // find conversation in conversations list and set current one to this contact, IF NOT FOUND REQUEST IT FROM SERVER
         console.log('set current conversation to contact: ', contact)
         let convs = context.state.conversations;

         let x = _.find(convs, c =>{

            return c.partyId1 === contact.id || c.partyId2 === contact.id

         })

         console.log('x', x)
         if(x){
            context.commit('setCurrentConversation', x);
         }
         else{

            context.dispatch('getConversationForContact', contact).then(conversation => {
                console.log('got conversation from server: ', conversation)
                context.commit('setCurrentConversation', conversation);
                convs.push(conversation);
                context.commit('setConversations', convs);
            })   

         }

    
    },

    getConversationForContact(context, contact){

        return contactService.getConversationForContact(contact);
    },

    setContactAsActive(context, contact){

        console.log('set active contact:' ,contact)
        context.commit('setActiveContact', contact)
        context.dispatch('setCurrentConversationToContact', contact)
    }

    
};


const mutations = {

    setName(state, name){

        state.name = name;
    },

    addLine(state, line){

        console.log('add line: ', line)
        state.conversation.push(line);
    },

    updateChatting(state, val){
        state.chatting = val;
    },


    setSearchUserResults(state, users){
        state.searchUsersResults = users;
    },

    setReceivedRequests(state, requests){
        state.receivedRequests = requests;
    },

    setMyContacts(state, contacts){
        state.myContacts = contacts;
    },

    setActiveContact(state, contact){
        state.activeContact = contact;
    },

    setCurrentConversation(state, conversation){
        state.currentConversation = conversation;
    },

    setUser(state, user){
        state.user = user;
    },

    setConversations(state, conversationList){
        state.conversations = conversationList;
    }
};

const getters = {

    activeConversationId: state => {

        const convs = state.conversations;
        let co = null;

        for (let index = 0; index < convs.length; index++) {
            const element = convs[index];
            
            if(element.partyId1 === state.activeContact.id || element.partyId2 === state.activeContact.id){
                co = element;
                break;
            }
        }

        return co;
    },

    findContactResults: state => {
        return state.searchUsersResults;
    },

    receivedRequests: state => {
        return state.receivedRequests;
    },

    myContacts: state => {
        return state.myContacts;
    },

    activeContact: state => {
        return state.activeContact;
    },

    currentConversation: state => {
        return state.currentConversation;
    },

    currentUser: state => {
        return state.user;
    }
}

export const chat = {

    namespaced: true,
    state,
    actions,
    mutations,
    getters
    
}
