<style>



</style>


<template>

<form>

    <div class="message-input">
        <div class="wrap">
            <input type="text" placeholder="Write your message..." v-model="message" />
            <i class="fa fa-paperclip attachment" aria-hidden="true"></i>
            <button class="submit" @click.prevent="sendMessage"><i class="fa fa-paper-plane" aria-hidden="true"></i></button>
        </div>
    </div>

</form>
</template>


<script>

    import { mapState, mapActions } from 'vuex'

    export default {

        name: 'MessageInput',
        props: [],
        components: {},

        data(){
            return {
                message: null
            }
        },

        computed: {
            ...mapState({
                activeContact: state => state.chat.activeContact,
                conversation: state => state.chat.currentConversation,
                account: state => state.account,
            }),

        },


        methods: {

            ...mapActions('chat', {
                sendToUser: 'sendToUser',
                setCurrentConversationToContact: 'setCurrentConversationToContact',
                
            }), 

            sendMessage(){

                if(this.message){

                    this.setCurrentConversationToContact(this.activeContact).then(() =>{
                        const mess = { 
                            conversationId: this.$store.getters['chat/currentConversation'].id,
                            message: this.message, 
                            toId: this.activeContact.id, 
                            toName: this.activeContact.fullName,
                            fromId: this.account.user.id,
                            fromName: this.account.user.firstName + ' ' + this.account.user.lastName,

                        
                        }
                        console.log('cr message: ', mess);

                        this.sendToUser(mess)
                        .then(() =>{
                            console.log('finsihed sending message')
                        })
                        
                        this.message = null;

                    })
                    
                    

                }
            },

        }
}

</script>