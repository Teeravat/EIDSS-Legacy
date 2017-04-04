package com.bv.eidss;

import android.content.ContentProvider;
import android.content.ContentValues;
import android.content.UriMatcher;
import android.database.Cursor;
import android.database.SQLException;
import android.database.sqlite.SQLiteDatabase;
import android.net.Uri;

import com.bv.eidss.data.EidssDatabase;

public class ASCampaignDiseasesProvider extends ContentProvider{

    static final String PROVIDER_NAME = "com.bv.eidss.ascampaigndeseasesprovider";
    static final String URL = "content://" + PROVIDER_NAME + "/ascampaigndeseases";
    public static final Uri CONTENT_URI = Uri.parse(URL);
    public static final String NAME = "name";

    static final int ASCAMPAIGNDESEASES = 1;
    static final int ASCAMPAIGNDESEASE_ID = 2;

    static final UriMatcher uriMatcher;
    static{
        uriMatcher = new UriMatcher(UriMatcher.NO_MATCH);
        uriMatcher.addURI(PROVIDER_NAME, "ascampaigndeseases", ASCAMPAIGNDESEASES);
        uriMatcher.addURI(PROVIDER_NAME, "ascampaigndeseases/#", ASCAMPAIGNDESEASE_ID);
    }

        private SQLiteDatabase db;

        @Override
        public boolean onCreate() {
            /*EidssDatabase mDb = new EidssDatabase(getContext());
            db = mDb.getReadableDatabase();
            return (db == null)? false:true;*/
            return true;
        }

        @Override
        public Uri insert(Uri uri, ContentValues values) {
            throw new SQLException("Failed to add a record into " + uri);
        }

        @Override
        public Cursor query(Uri uri, String[] projection, String selection,
                            String[] selectionArgs, String sortOrder) {
            if (db == null) {
                EidssDatabase mDb = new EidssDatabase(getContext());
                db = mDb.getReadableDatabase();
            }
            switch (uriMatcher.match(uri)) {
                case ASCAMPAIGNDESEASES:
                    return db.rawQuery(EidssDatabase.select_sql_campaign_diagnosis_list, selectionArgs);
                case ASCAMPAIGNDESEASE_ID:
                    return db.rawQuery(EidssDatabase.select_sql_campaign_diagnosis_one, selectionArgs);
                default:
                    throw new IllegalArgumentException("Wrong URI: " + uri);
            }
        }

        @Override
        public int delete(Uri uri, String selection, String[] selectionArgs) {
            throw new IllegalArgumentException("Unknown URI " + uri);
        }

        @Override
        public int update(Uri uri, ContentValues values, String selection,
                          String[] selectionArgs) {
            throw new IllegalArgumentException("Unknown URI " + uri );
        }

        @Override
        public String getType(Uri uri) {
            throw new IllegalArgumentException("Unsupported URI: " + uri);
        }
}