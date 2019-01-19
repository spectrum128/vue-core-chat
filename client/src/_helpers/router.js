import Vue from 'vue';
import Router from 'vue-router';

import HomePage from '../home/HomePage'
import LoginPage from '../login/LoginPage'
import RegisterPage from '../register/RegisterPage'
import ChatTest from '../chat/ChatTest'
import ChatFrame from '../chat/ChatFrame'
import FindUser from '../chat/FindUser'
import ReceivedRequests from '../chat/ReceivedRequests'

Vue.use(Router);

export const router = new Router({
  mode: 'history',
  routes: [
    { path: '/', component: ChatFrame },
    { path: '/login', component: LoginPage },
    { path: '/register', component: RegisterPage },
    { path: '/admin', component: HomePage },
    { path: '/chattest', component: ChatTest },
    { path: '/addcontact', component: FindUser },
    { path: '/receivedrequests', component: ReceivedRequests },

    // otherwise redirect to home
    { path: '*', redirect: '/' }
  ]
});

router.beforeEach((to, from, next) => {
  // redirect to login page if not logged in and trying to access a restricted page
  const publicPages = ['/login', '/register'];
  const authRequired = !publicPages.includes(to.path);
  const loggedIn = localStorage.getItem('user');

  if (authRequired && !loggedIn) {
    return next('/login');
  }

  next();
})