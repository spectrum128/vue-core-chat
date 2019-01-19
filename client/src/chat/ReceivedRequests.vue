<style>



</style>


<template>
<div id="frame">

    <div id="finduserpanel">

        <div id="search" class="mb-5">
            <label for="" @click="goBack"><i class="fa fa-times mr-3" aria-hidden="true"></i><span class="applabel">The people below would like to add you as a contact...</span></label>
        </div>

        <div v-if="!receivedRequests || receivedRequests.length === 0" class="px-3 py-3">
            No pending contact requests...
        </div>

        <div>
           
            <ul v-for="contact in receivedRequests" :key="contact.id">
                <li class="contact">
					<find-user-result :contact="contact" :action="'accept'" ></find-user-result>
				</li>
            </ul>
        </div>
    </div>


</div>
</template>


<script>

    import _ from 'lodash'
    import { mapState, mapActions } from 'vuex'
    import FindUserResult from './FindUserResult.vue'

    export default {

        name: 'ReceivedRequests',
        props: [],
        components: {FindUserResult},


        data(){

            return {

                
                

            }


        },

        
        computed:{

            receivedRequests(){

                return this.$store.getters["chat/receivedRequests"];
            }
        },


        created(){

            
        },



        mounted(){
            this.getReceivedRequests();
        },


        methods: {

            ...mapActions('chat', ['getReceivedRequests']),

            goBack(){

                this.$router.push({path: '/chatroom'});
            }



        }
}

</script>