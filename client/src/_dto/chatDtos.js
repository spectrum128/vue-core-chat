

export {

    
    Conversation,
    ContactMessage
    
}


function Conversation(id, userid, partyid){

    return {
        id: id,
        partyid1: userid,
        partyid2: partyid,
        messages: []
    }
   
}


function ContactMessage(conversatinoId, from, to, message){

    return {
        id: null,
        conversationId: conversatinoId || null,
        toName: to && to.fullName || null,
        toId: to && to.id || null,
        message: message || null,
        fromName: from && from.fromName || null,
        fromId: from && from.id || null,
        sentOn: null,
    }
    
}