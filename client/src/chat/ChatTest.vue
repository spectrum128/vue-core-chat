<style>



</style>


<template>
<div>


<div class="container">

<div class="row">
    <h4>Chat app</h4>
    <div>
            <h4>Other links</h4>
            <ul>
                <li>
                    <router-link to="/home">Home</router-link>
                </li>
                <li>
                    <router-link to="/login">Logout</router-link>
                </li>
            </ul>
        </div>
</div>

<div class="row mt-5">
    <div class="col-12">

        <!-- <div id="entrance" class="mt-2" v-if="!chatStarted">
            <label for="nick">Enter your nickname:</label>
            <input type="text" id="nick" v-model="name"/>
            <button @click="startChat" class="btn btn-success">Continue</button>
        </div> -->


        <h1>Start chatting {{account.user.firstName}}!</h1>

        

        <div id="chat" v-if="chatStarted">

            <h3 id="spn-nick"></h3>
            <form id="frm-send-message" action="#">

                <div class="form-group">
                    <label for="message">Message:


                        <input type="text" id="message" v-model="message" class="form-control"/>
                    </label>
                <input type="submit" id="send" value="Send" class="btn btn-primary" @click.prevent="sendMessage" />

                </div>
                
            </form>

            <div class="clear">

            </div>

            <ul id="messages" class="mt-5" >
                <li v-for="conv in conversation" :key="conv">{{conv}}</li>
            </ul>
        </div>

    </div>
</div>

</div>



</div>
</template>


<script>

import { mapState, mapActions } from 'vuex'

export default {

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
            chatStarted: state => state.chat.chatting
        }),


        
    },

    created(){
        this.startChat()
    },

    mounted () {
        
        
    },


    methods: {

        startChat(){

            
            console.log('attempting to start chat...');

            const user = JSON.parse(localStorage.getItem('user'));

            console.log('got user: ', user);

            this.start({ name: this.account.user.firstName, token: user.token });


            console.log('chat started...')
            
        
        

        },

        sendMessage(){

            if(this.message){

                console.log('cr message: ', this.message);

                this.send(this.message)
                .then(() =>{
                    console.log('in promise')
                })
                // .then(() => {
                    this.message = null;
                // });


            }
            

        },

        ...mapActions('chat', {
            send: 'send',
            start: 'start'
        })

    },


   
};

</script>