<template>
    <div id="frame">

        
        <div id="content">

<h1>Hi {{account.user.firstName}}!</h1>
        
        <h3>Existing Users</h3>
        <em v-if="users.loading">Loading users...</em>
        <span v-if="users.error" class="text-danger">ERROR: {{users.error}}</span>
        <ul v-if="users.items">
            <li v-for="user in users.items" :key="user.id">
                {{user.firstName + ' ' + user.lastName}}
                <span v-if="user.deleting"><em> - Deleting...</em></span>
                <span v-else-if="user.deleteError" class="text-danger"> - ERROR: {{user.deleteError}}</span>
                <span v-else> 
                    - <a @click="deleteUser(user.id)" class="text-danger">Delete</a> 
                    - <a @click="sendContactRequest(user.id)" class="">Send Contact Request</a>
                </span>

            </li>
        </ul>
        <div>
            <h4>Other links</h4>
            <ul>
                <li>
                    <router-link to="/chatroom">Go to Chatroom</router-link>
                </li>
                <li>
                    <router-link to="/chattest">Go to Chat test</router-link>
                </li>
                <li>
                    <router-link to="/login">Logout</router-link>
                </li>
            </ul>
        </div>

        </div>
        
    </div>
</template>

<script>
import { mapState, mapActions } from 'vuex'

export default {
    computed: {
        ...mapState({
            account: state => state.account,
            users: state => state.users.all
        })
    },
    created () {
        this.getAllUsers();
    },
    methods: {
        ...mapActions({
            getAllUsers: 'users/getAll',
            deleteUser: 'users/delete',
            sendContactRequest: 'chat/sendContactRequest'
        }),

       
    }
};
</script>