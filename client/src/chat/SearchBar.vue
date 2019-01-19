<style>



</style>


<template>

<div id="search">
    <label for=""><i class="fa fa-search" aria-hidden="true"></i></label>
    <input type="text" v-model="searchname" @input="updateSearch" placeholder="Search contacts..." />
</div>

</template>


<script>

    export default {

        name: 'SearchBar',
        props: ["value"],
        components: {},


        data(){

            return {

                searchname: this.value
            }


        },



        created(){

        },



        mounted(){

        },


        methods: {

            updateSearch(){
                console.log('searching: ', this.searchname)
                this.$emit('input', this.searchname)
            }



        }
}

</script>