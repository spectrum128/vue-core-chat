<style>



</style>


<template>
<div id="frame">

    <div id="finduserpanel">

        <div id="search">
            <label for="" @click="cancelSearch"><i class="fa fa-times" aria-hidden="true"></i></label>
            <input type="text" placeholder="Find user..." v-model="searchname" />
            <label for="" @click="findUser"><i class="fa fa-search" aria-hidden="true"></i></label>
        </div>

        <div v-if="searchDirty" class="px-3 py-3">Searching...</div>
        <div v-else>
            <div v-if="searchname && (!searchResults || searchResults.length === 0)">
                <p class="mx-4 my-4"><em>No people with that name found</em></p>
            </div>
            <ul v-for="contact in searchResults" :key="contact.id">
                <li class="contact">
					<find-user-result :contact="contact" ></find-user-result>
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

        name: 'FindUser',
        props: [],
        components: {FindUserResult},


        data(){

            return {

                searchname: null,
                searchDirty: false,
                isFetching: false,
                

            }


        },

        watch:{

            searchname: function(value){
                
                this.searchDirty = true;


                this.debouncedSearch();
            }
        },

        computed:{

            searchResults(){

                return this.$store.getters["chat/findContactResults"];
            }
        },


        created(){

            this.debouncedSearch = _.debounce(this.findUser, 500);
        },



        mounted(){

        },


        methods: {

            ...mapActions('chat', ['findContacts']),

            findUser(){

                var self = this;
                this.searchDirty = false;
                

                

                    console.log('find user ' + self.searchname);

                    this.findContacts(self.searchname);
                

            },

            cancelSearch(){

                this.$router.push({path: '/chatroom'})
            }


        }
}

</script>