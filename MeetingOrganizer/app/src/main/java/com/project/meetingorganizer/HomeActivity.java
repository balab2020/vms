package com.project.meetingorganizer;

import android.content.Intent;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

public class HomeActivity extends AppCompatActivity {

    private TextView txtOrganizorName;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_home);
        Toolbar toolbar = findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);

        FloatingActionButton fab = findViewById(R.id.fab);
        fab.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Snackbar.make(view, "Replace with your own action", Snackbar.LENGTH_LONG)
                        .setAction("Action", null).show();
            }
        });

        txtOrganizorName = (TextView)findViewById(R.id.txtWelcomeOrganizor);

        txtOrganizorName.setText(UserProvider.getInstance().getUser().getUserName());

        Button btnCreateMeeting = (Button)findViewById(R.id.btnHomeCreateMeeting);

        Button btnApproveMeeting = (Button)findViewById(R.id.btnHomeApproveMeeting);

        final AppCompatActivity thisActivity = this;

        String message = getIntent().getStringExtra("Message");

        if(message != null && message != "")
        {
            int duration = Toast.LENGTH_SHORT;

            Toast toast = Toast.makeText(getApplicationContext(), message, duration);
            toast.show();
        }

        btnCreateMeeting.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent meetings = new Intent(thisActivity, MeetingsActivity.class);
                startActivity(meetings);
            }
        });

        btnApproveMeeting.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent meetings = new Intent(thisActivity, ApproveMeetingActivity.class);
                startActivity(meetings);
            }
        });
    }

}
