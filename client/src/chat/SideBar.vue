<style>



</style>


<template>

<div id="sidepanel">
		
        <profile :user="account.user"></profile>

		<search-bar v-model="searchname"></search-bar>

		<div id="contacts">
			<ul v-for="contact in myContacts" :key="contact.id">	
				
					<contact-preview :contact="contact" @contactSelected="contactSelected" ></contact-preview>
				
			</ul>
		</div>

		<div id="bottom-bar">
			<button id="addcontact" @click="addContact"><i class="fa fa-user-plus fa-fw" aria-hidden="true"></i> <span>Add contact</span></button>
			<button id="settings" @click="viewReceivedRequests"><i class="fa fa-cog fa-fw" aria-hidden="true"></i> <span>Contact Requests ({{contactrequests.length}})</span></button>
		</div>
	</div>


</template>


<script>

    import Profile from './Profile.vue'
    import ContactPreview from './ContactPreview.vue'
    import SearchBar from './SearchBar.vue'
    import { mapState, mapActions } from 'vuex'
    import _ from 'lodash'

    export default {

        name: 'SideBar',
        props: [],
        components: {Profile, ContactPreview, SearchBar},

        data(){

            return {
                searchname: null
            }


        },


        computed: {

            ...mapState({
                account: state => state.account,
                contactrequests: state => state.chat.receivedRequests
            }),

            myContacts: function() { 

                    var self = this;
                    if(!this.searchtext){
                        return this.$store.getters['chat/myContacts']
                    }
                    else{
                        return _.filter(this.$store.getters['chat/myContacts'], function(x){
                            let sr = self.searchtext.toLowerCase();
                            return x.fullName.toLowerCase().indexOf(sr) > -1;
                        })
                    }
                    
            },

            searchtext: function(){
                return this.searchname
            }
        },


        created(){
            this.getMyContacts();
            this.getReceivedRequests();
        },



        methods: {

            ...mapActions('chat', ['getMyContacts', 'setContactAsActive', 'getReceivedRequests']),

            addContact(){

                this.$router.push({ path: '/addcontact' })
            },

            viewReceivedRequests(){
                this.$router.push({ path: '/receivedrequests'})
            },

            contactSelected(con){
                console.log('contact selected: ', con.fullName);
                this.setContactAsActive(con);
            }



        }
}

</script>