package com.project.meetingorganizer;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.util.JsonReader;
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
import com.google.zxing.BarcodeFormat;

import org.json.JSONStringer;

import java.util.Arrays;

public class ApproveMeetingActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_approve_meeting);
        Button btnScanner = findViewById(R.id.btnScanBarcode);
        final AppCompatActivity thisActivity = this;
        final EditText txtScannedCode = findViewById(R.id.btnScannedBarcode);
        final EditText txtMeetingId = findViewById(R.id.btnApproveMeetingId);
        btnScanner.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                IntentIntegrator integrator = new IntentIntegrator(thisActivity);
                integrator.initiateScan();
            }
        });

        Button btnApproveMeeting = findViewById(R.id.btnApproveMeeting);
        final Activity self= this;

        btnApproveMeeting.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                final String meetingId = txtMeetingId.getText().toString();
                final String barcode = txtScannedCode.getText().toString();

                if(meetingId != null && meetingId != "" && barcode != null && barcode != ""){

                    RequestQueue requestQueue = Volley.newRequestQueue(getApplicationContext());
                    String vmsDomainUri = ServerInfoProvider.getInstance().getVmsServiceApiUri();
                    String url = vmsDomainUri + "meeting/Complete/"+meetingId+"?otp=" + barcode;

                    CharSequence text = "Approving meeting..";
                    int duration = Toast.LENGTH_LONG;
                    Toast toast = Toast.makeText(getApplicationContext(), text, duration);
                    toast.show();

                    StringRequest postRequest = new StringRequest(Request.Method.PUT, url,
                            new Response.Listener<String>()
                            {
                                @Override
                                public void onResponse(String response) {
                                    // response
                                    Intent meetings = new Intent(self, HomeActivity.class);
                                    meetings.putExtra("Message", "Meeting approved successfully!!!");
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
                    ){};
                    //----------------POST---------------------------
                    requestQueue.add(postRequest);
                }
            }
        });
    }

    public void onActivityResult(int requestCode, int resultCode, Intent intent) {
        IntentResult scanResult = IntentIntegrator.parseActivityResult(requestCode, resultCode, intent);
        if (scanResult != null) {
            // handle scan result
            String data = scanResult.getContents();
            String[] parsed =  data.split(",");
            EditText txtScannedCode = findViewById(R.id.btnScannedBarcode);
            EditText txtMeetingId = findViewById(R.id.btnApproveMeetingId);
            txtMeetingId.setText(parsed[0]);
            txtScannedCode.setText(parsed[1]);
        }
        // else continue with any other code you need in the method
    }

}
