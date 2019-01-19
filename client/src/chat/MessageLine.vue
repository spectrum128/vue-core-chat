<style>



</style>


<template>
<div class="">

    <img :src="message.img" alt="" v-if="message.img" />
    
    <p><span v-if="message.fromName"><strong>{{message.fromName + ': '}}</strong></span>{{message.message}}</p>

</div>
</template>


<script>

    export default {

        name: 'MessageLine',
        props: ['message'],
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