package com.project.meetingorganizer;
import android.content.Intent;
import android.os.AsyncTask;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.text.TextUtils;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest ;
import com.android.volley.toolbox.Volley;
import com.project.meetingorganizer.Models.UserModel;

import org.json.JSONException;
import org.json.JSONObject;

public class MainActivity extends AppCompatActivity {

    private EditText txtUserName;

    private EditText txtPassword;

    private EditText txtServerIp;

    private UserLoginTask userLoginTask;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        txtUserName = (EditText) findViewById(R.id.txtUserName);

        txtPassword = (EditText) findViewById(R.id.txtPassword);

        txtServerIp = findViewById(R.id.txtServerIP);

        txtUserName.setText("vijay@psna.com");

        txtPassword.setText("password123");

        Button btnLogin = (Button) findViewById(R.id.btnLogin);

        btnLogin.setOnClickListener(new View.OnClickListener(){

            @Override
            public void onClick(View view) {

                attemptLogin();
            }
        });
    }

    private void attemptLogin(){
        String userName = this.txtUserName.getText().toString();
        String password = txtPassword.getText().toString();
        ServerInfoProvider.getInstance().setServerIP(txtServerIp.getText().toString());
        if (TextUtils.isEmpty(userName) || TextUtils.isEmpty(password)){
            return;
        }

        CharSequence text = "Logging in, please wait..";
        int duration = Toast.LENGTH_LONG;
        Toast toast = Toast.makeText(getApplicationContext(), text, duration);
        toast.show();
        final AppCompatActivity loginActivity = this;
        final Button btnLogin = (Button) findViewById(R.id.btnLogin);
        btnLogin.setEnabled(false);
        userLoginTask = new UserLoginTask(userName, password, new PostLoginAction() {
            @Override
            public void postLogin(boolean result) {
                if (result) {
                    Intent meetings = new Intent(loginActivity, HomeActivity.class);
                    startActivity(meetings);
                }
                btnLogin.setEnabled(true);
            }
        });

        userLoginTask.execute((Void)null);
    }

    interface PostLoginAction {

        void postLogin(boolean result);
    }


    class UserLoginTask extends AsyncTask<Void, Void, Boolean>
    {

        private final String userName;

        private final String password;

        private final PostLoginAction postLoginAction;

        UserLoginTask(String username, String password, PostLoginAction postLoginAction){
            this.userName = username;
            this.password = password;
            this.postLoginAction = postLoginAction;
        }

        @Override
        protected Boolean doInBackground(Void... voids)
        {
            try {
                String vmsDomainUri = ServerInfoProvider.getInstance().getVmsServiceApiUri();
                String url = vmsDomainUri + "login/login?email=" + userName +"&password=" + password;
                RequestQueue queue = Volley.newRequestQueue(getApplicationContext());
                // Request a string response from the provided URL.
                JsonObjectRequest jsonObjectRequest = new JsonObjectRequest
                        (Request.Method.GET, url, null, new Response.Listener<JSONObject>() {
                            @Override
                            public void onResponse(JSONObject response) {
                                try {
                                    if(response.has("IsAuthenticated") && response.getBoolean("IsAuthenticated")){
                                        UserModel user = new UserModel();
                                        user.setUserId(response.getInt("UserId"));
                                        user.setUserName(response.getString("UserName"));
                                        UserProvider.getInstance().setUser(user);
                                        postLoginAction.postLogin(true);
                                    } else {
                                        postLoginAction.postLogin(false);
                                    }
                                } catch (JSONException e) {

                                }
                            }
                        }, new Response.ErrorListener() {
                            @Override
                            public void onErrorResponse(VolleyError error) {
                                // TODO: Handle error
                                String err = error.toString();
                            }
                        });
                queue.add(jsonObjectRequest);
                return false;
            } catch (Exception e) {
                return false;
            }
        }

        @Override
        protected void onPostExecute(final Boolean success)
        {
            postLoginAction.postLogin(success);
        }

        @Override
        protected void onCancelled()
        {

        }
    }

}


