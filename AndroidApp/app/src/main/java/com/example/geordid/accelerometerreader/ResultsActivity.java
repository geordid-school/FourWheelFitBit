package com.example.geordid.accelerometerreader;

import android.content.Intent;
import android.os.Bundle;
import android.app.Activity;
import android.text.format.DateFormat;
import android.util.Log;
import android.view.LayoutInflater;
import android.widget.TableLayout;
import android.widget.TableRow;
import android.widget.TextView;

import org.json.JSONArray;
import org.json.JSONObject;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.time.Instant;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.ZoneId;
import java.util.Date;
import java.util.Locale;

public class ResultsActivity extends Activity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_results);
        getActionBar().setDisplayHomeAsUpEnabled(true);

        try {
            Intent intent = getIntent();
            JSONObject jObj = new JSONObject(intent.getStringExtra("data"));
            JSONArray array = jObj.getJSONArray("results");
            TableLayout table = (TableLayout)findViewById(R.id.table);

            for(int i = 0; i < array.length(); i++) {
                JSONObject currentObj = array.getJSONObject(i);
                TableRow row = (TableRow) LayoutInflater.from(this).inflate(R.layout.attr_tablerow, null);
                ((TextView)row.findViewById(R.id.trStart)).setText(getFormattedDate(currentObj.getLong("startTime")));
                ((TextView)row.findViewById(R.id.trEnd)).setText(getFormattedDate(currentObj.getLong("endTime")));
                ((TextView)row.findViewById(R.id.trMoving)).setText(currentObj.getString("isMoving"));
                table.addView(row);
            }
            table.requestLayout();
        } catch(Exception e) {
            //dez didn't make the catch
            Log.e("ResultsActivity", e.getMessage());
        }
    }

    @Override
    public void onBackPressed() {
        startActivity(new Intent(this, MainActivity.class));
    }

    private String getFormattedDate(long millis) {
        return new SimpleDateFormat("hh:mm:ss.SSS", Locale.US).format(millis);
    }
}
