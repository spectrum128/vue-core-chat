<style>



</style>


<template>
<div>


    <div class="contact-profile" v-if="user.profileImgUrl">
        <img :src="user.profileImgUrl" alt="" v-if="user.profileImgUrl"/>
        
        
        <div class="social-media">
            
        </div>
    </div>

    <div class="contact-profile" v-if="!user.profileImgUrl">

        <span v-if="!user.profileImgUrl"><i class="fa fa-user fa-15x ml-3 mr-3" aria-hidden="true"></i>{{user.firstName + " " + user.lastName}}</span>

    </div>

</div>


</template>


<script>

    export default {

        name: 'ContactHeader',
        props: ['user'],
        components: {},


        data(){

            return {


            }


        },



        created(){

        },



        mounted(){

        },


        methods: {





        }
}

</script>