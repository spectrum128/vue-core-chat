<style>



</style>


<template>
<li class="contact" :class="{ 'active': isActive }" @click="selected">
<div class="wrap">

    <div v-if="!inline" >

        <span v-if="!hidestatus" :class="{ 'contact-status': true, 'online': contact.online }">
        <img :src="contact.profileImgUrl" alt="" v-if="contact.profileImgUrl" />
        <span v-if="!contact.profileImgUrl" :class="{ 'online': contact.online }"><i :class="{ 'online': contact.online }" class="fa fa-user fa-15x ml-2" aria-hidden="true"></i></span>
        </span>
        <div class="meta">
            
            <p class="name ml-5" :class="{ 'online': contact.online }">{{contact.fullName}}</p>
            <p class="preview">{{preview}}</p>
        </div>

    </div>
    <div v-else>

        
        <div class="meta">
            <div class="mr-5">

                <span v-if="!hidestatus" :class="{ 'contact-status': true, 'online': contact.online }"></span>
                <img :src="contact.profileImgUrl" alt="" v-if="contact.profileImgUrl"/>
                <span v-else><i class="fa fa-user fa-15x" aria-hidden="true"></i></span>
                <span class="name px-3 py-3">{{contact.fullName}}</span>
                <span class="preview">{{preview}}</span>

            </div>
            
        </div>


    </div>
    
</div>
</li>
</template>


<script>

    export default {

        name: 'ContactPreview',
        props: [ 'contact', 'preview', 'hidestatus', 'inline'],
        components: {},


        data(){

            return {


            }


        },



        created(){

        },



        mounted(){

        },


        computed: {

            isActive(){

                const  c = this.$store.getters['chat/activeContact'];

                return c && c.id === this.contact.id;
            }
        },


        methods: {


            selected(){
                console.log('selected contact: ', this.contact)
                this.$emit('contactSelected', this.contact)
            }


        }
}

</script>