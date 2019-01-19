<style>



</style>


<template>

<div class="messages">
			<ul v-for="message in messages" :key="message.message">
				<li :class="{ 'sent' : message.fromId === account.user.id, 'replies': message.fromId != account.user.id }">
                    <message-line :message="message"></message-line>
                </li>
				
			</ul>
</div>

</template>


<script>

    import MessageLine from './MessageLine'
    import { mapState, mapActions } from 'vuex'

    export default {

        name: 'MessageList',
        props: ['messages'],
        components: {MessageLine},


        data(){

            return {


            }


        },


        computed: {

            ...mapState({
                account: state => state.account,
                myContacts: state => state.chat.myContacts
            }),
        },

        created(){

        },



        mounted(){

        },


        methods: {





        }
}

</script>