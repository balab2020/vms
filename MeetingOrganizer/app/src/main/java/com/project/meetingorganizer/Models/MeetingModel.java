package com.project.meetingorganizer.Models;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.Date;

public class MeetingModel
{
    public final static MeetingModel ParseFromJson(JSONObject jsonObject) throws JSONException {
        MeetingModel meeting = new MeetingModel();
        meeting.VisitorEmail = jsonObject.getString("visitorEmail");
        meeting.Phone = jsonObject.getString("mobile");
        meeting.Remarks = jsonObject.getString("purpose");
        meeting.ScheduledDateString = jsonObject.getString("dateTime");
        meeting.Id = jsonObject.getInt("meetingId");
        meeting.OrganizorId= jsonObject.getInt("organizorId");
        return meeting;
    }

    public  Integer Id;

    public String VisitorEmail;

    public Date ScheduledDate;

    public String ScheduledDateString;

    public String Remarks;

    public Integer OrganizorId;

    public String Phone;
}
