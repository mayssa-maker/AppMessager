<script>
  import {onMount} from 'svelte';
  import {authenticated} from '../stores/auth';
  import axios from 'axios';
    
    let message = ''

    onMount(async () => {
        try {
            const response = await axios.get('https://localhost:7100/Auth/conversations', {
                headers: {'Content-Type': 'application/json'},
                credentials: 'include',
            });


            const content = await response.json();

           
            authenticated.set(true);
        } catch (e) {
            message = 'You are not logged in';
            authenticated.set(false);
        }
    });
</script>

{message}