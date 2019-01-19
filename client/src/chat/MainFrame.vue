<style>



</style>


<template>

<div class="content">
		
        <contact-header :user="account.user"></contact-header>

		<message-list :messages="messages"></message-list>

        <message-input></message-input>
		
	</div>


</template>


<script>


    import ContactHeader from './ContactHeader.vue'
    import MessageLine from './MessageLine.vue'
    import MessageInput from './MessageInput.vue'
    import MessageList from './MessageList.vue'
    import { mapState, mapActions } from 'vuex'

    export default {

        name: 'MainFrame',
        props: [],
        components: {ContactHeader, MessageLine, MessageInput, MessageList},


        data(){

            return {
                message: null,
                

            }
        },


        methods: {

        },

        computed: {
            ...mapState({
                account: state => state.account,
                messages: state => { return state.chat.currentConversation && state.chat.currentConversation.messages || []}
            }),

        },


}

</script>