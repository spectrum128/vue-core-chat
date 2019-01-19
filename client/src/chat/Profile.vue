<style>



</style>


<template>

<div id="profile">
			<div class="wrap">
                <span v-if="!user.profileImgUrl"><i class="fa fa-user fa-15x" aria-hidden="true"></i></span>
				<img id="profile-img" :src="user.profileImgUrl" class="online" alt="" v-if="user.profileImgUrl"/>
				<span class="px-3 py-3">{{user.firstName + " " + user.lastName}}</span>
				<span style="float: right"><router-link to="/login">Sign out</router-link></span>
				<div id="status-options">
					<ul>
						<li id="status-online" class="active"><span class="status-circle"></span> <p>Online</p></li>
						<li id="status-away"><span class="status-circle"></span> <p>Away</p></li>
						<li id="status-busy"><span class="status-circle"></span> <p>Busy</p></li>
						<li id="status-offline"><span class="status-circle"></span> <p>Offline</p></li>
					</ul>
				</div>
				
			</div>
		</div>

</template>


<script>

    export default {

        name: 'Profile',
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