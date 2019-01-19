<style>



</style>


<template>

<div id="frame">

    <side-bar></side-bar>
    <main-frame></main-frame>

</div>

</template>


<script>

    import SideBar from './SideBar.vue'
    import MainFrame from './MainFrame.vue'
    import { mapState, mapActions } from 'vuex'

    export default {

        name: 'ChatFrame',
        props: [],
        components: {SideBar, MainFrame},

        data(){

        return {

            name: null,
            message: null,
            

        }
    },

    computed: {
        ...mapState({
            account: state => state.account,
            conversation: state => state.chat.conversation,
            chatStarted: state => state.chat.chatting,
            activeContact: state => state.chat.activeContact
        }),


        
    },

    

    mounted () {
        
        this.startChat()
    },


    methods: {

        startChat(){

            
            console.log('attempting to start chat...');
            
            const user = JSON.parse(localStorage.getItem('user'));

            console.log(user)
            

            console.log('got user: ', user);

            this.start(this.account.user);


            console.log('chat started...')
            
        
        

        },

        sendMessage(){

            if(this.message){

                console.log('cr message: ', this.message);

                this.sendToUser({ to: this.activeContact.id, message: this.message})
                .then(() =>{
                    console.log('finsihed sending message')
                })
                // .then(() => {
                    this.message = null;
                // });


            }
            

        },

        ...mapActions('chat', {
            send: 'send',
            sendToUser: 'sendToUser',
            start: 'start'
        })

    },


        
}

</script>