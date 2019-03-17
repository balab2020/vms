package com.project.meetingorganizer;

import android.app.Activity;
import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import com.android.volley.AuthFailureError;
import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;
import com.project.meetingorganizer.Models.MeetingModel;
import com.project.meetingorganizer.Models.UserModel;

import org.json.JSONException;
import org.json.JSONObject;

public class MeetingsActivity extends AppCompatActivity {

    private UserModel user;

    private EditText txtVisitorEmail;

    private EditText txtDate;

    private EditText txtTime;

    private EditText txtRemarks;

    private EditText txtPhone;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_meetings);
        user = UserProvider.getInstance().getUser();
        txtDate = (EditText) findViewById(R.id.txtDate);
        txtRemarks = (EditText)findViewById(R.id.txtRemarks);
        txtTime = (EditText)findViewById(R.id.txtTime);
        txtPhone = (EditText)findViewById(R.id.txtPhone);
        txtVisitorEmail = (EditText)findViewById(R.id.txtVisitorEmail);
        Button btnCreate = (Button) findViewById(R.id.btnCreate);
        Button btnCancel = (Button) findViewById(R.id.btnCancel);
        final Activity self= this;
        btnCreate.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                MeetingModel meeting = CreateMeetingModelFromView();

                JSONObject jsonMeetingObject = new JSONObject();

                try {
                    jsonMeetingObject.put("organizorId",meeting.OrganizorId);
                    jsonMeetingObject.put("visitorEmail",meeting.VisitorEmail);
                    jsonMeetingObject.put("mobile",meeting.Phone);
                    jsonMeetingObject.put("purpose",meeting.Remarks);
                    jsonMeetingObject.put("dateTime","2019-04-23T18:25:43.511Z");
                    final String jsonBody = jsonMeetingObject.toString();

                    RequestQueue requestQueue = Volley.newRequestQueue(getApplicationContext());
                    String vmsDomainUri = ServerInfoProvider.getInstance().getVmsServiceApiUri();
                    String url = vmsDomainUri + "meeting/create";
                    CharSequence text = "Creating meeting..";
                    int duration = Toast.LENGTH_LONG;

                    Toast toast = Toast.makeText(getApplicationContext(), text, duration);
                    toast.show();
                    //----------------POST---------------------------
                    StringRequest postRequest = new StringRequest(Request.Method.POST, url,
                            new Response.Listener<String>()
                            {
                                @Override
                                public void onResponse(String response) {
                                    // response
                                    Intent meetings = new Intent(self, HomeActivity.class);
                                    meetings.putExtra("Message", "Meeting created successfully & notification sent");
                                    startActivity(meetings);
                                                                        Log.d("Response", response);
                                }
                            },
                            new Response.ErrorListener()
                            {
                                @Override
                                public void onErrorResponse(VolleyError error) {
                                    // error
                                    Log.d("Error.Response", error.toString());
                                }
                            }
                    ){
                        @Override
                        public String getBodyContentType() {
                            return "application/json";
                        }

                        @Override
                        public byte[] getBody() throws AuthFailureError {
                            return jsonBody.getBytes();
                        }
                    };
                    //----------------POST---------------------------
                    requestQueue.add(postRequest);
                } catch (JSONException e) {
                    e.printStackTrace();
                }
            }
        });
    }

    private MeetingModel CreateMeetingModelFromView()
    {
        MeetingModel meeting = new MeetingModel() ;
        meeting.VisitorEmail = txtVisitorEmail.getText().toString();
        meeting.OrganizorId = user.getUserId();
        meeting.Phone = txtPhone.getText().toString();
        meeting.ScheduledDateString = txtDate.getText().toString() + " " + txtTime.getText().toString();
        meeting.Remarks = txtRemarks.getText().toString();
        return meeting;
    }
}
