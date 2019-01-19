<style>



</style>


<template>

<div class="wrap">

           
    <div class="meta">
        <div class="mr-5">

            <div class="row">

                <div class="col-9 pt-2">
                    <img :src="contact.profileImgUrl" alt="" v-if="contact.profileImgUrl" class="small-profile-pic"/>
                    <span v-else><i class="fa fa-user fa-15x" aria-hidden="true"></i></span>
                    <span class="name px-3 py-3">{{contact.fullName}}</span>
                </div>
                <div class="col-3">
                    <button v-if="contactAction === 'send'" @click="sendContactRequest" class="appbutton icon-text-button"><i class="fa fa-user-plus fa-fw mr-2" aria-hidden="true"></i> <span>Send contact request</span></button>
                    <button v-if="contactAction === 'accept'" @click="acceptRequest" class="appbutton icon-text-button"><i class="fa fa-user-plus fa-fw mr-2" aria-hidden="true"></i> <span>Accept request</span></button>
                </div>

            </div>
            

            

        </div>
        
    </div>


    
    
</div>

</template>


<script>

    export default {

        name: 'FindUserResult',
        props: ['contact', 'action'],
        components: {},


        data(){

            return {

                contactAction: this.action || 'send'
            }


        },



        created(){

        },



        mounted(){

        },


        methods: {


            sendContactRequest(){

                this.$store.dispatch('chat/sendContactRequest', this.contact.id).then(() => {

                    this.$store.dispatch('chat/clearFindContactResults');

                    this.$router.push({path: '/chatroom'})
                })
            },

            acceptRequest(){

                this.$store.dispatch('chat/acceptContactRequest', this.contact).then(() =>{

                    this.$router.push({path: '/chatroom'});
                })
            }


        }
}

</script>