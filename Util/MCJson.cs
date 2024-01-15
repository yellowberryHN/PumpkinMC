﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PumpkinMC.Util
{
    public class MCJson
    {
        public override string ToString()
        {
            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
                IncludeFields = true
            };

            return JsonSerializer.Serialize((object)this, options);
        }
    }
    public class MCJsonStringClickEvent : MCJson
    {
        public string open_url;
        public string run_command;
        public string suggest_command;
        public string change_page;
    }
    public class MCJsonStringHoverEvent : MCJson
    {
        public string show_text;
        public string show_item;
        public string show_entity;
    }

    public class MCJsonString : MCJson
    {
        public string text;
        public bool bold;
        public bool italic;
        public bool underlined;
        public bool strikethrough;
        public bool obfuscated;
        public string color;
        public string insertion;

        public MCJsonStringClickEvent click_event;
        public MCJsonStringHoverEvent hover_event;

        public List<MCJsonString> extra;

        public MCJsonString(string text = null)
        {
            this.text = text;
        }
    }

    public class MCJsonTranslateString : MCJson
    {
        public string translate;
        public List<MCJsonString> with;
    }

    public class ServerStatusPlayer : MCJson
    {
        public string name;
        public string id;

        public ServerStatusPlayer(string name, string id)
        {
            this.name = name;
            this.id = id;
        }
    }

    public class ServerStatusVersion : MCJson
    {
        public string name;
        public int protocol;

        public ServerStatusVersion(string name, int protocol)
        {
            this.name = name;
            this.protocol = protocol;
        }
    }
    public class ServerStatusPlayerInfo : MCJson
    {
        public int max;
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public int online;
        public List<ServerStatusPlayer> sample;

        public ServerStatusPlayerInfo(int max, int online, List<ServerStatusPlayer> sample)
        {
            this.max = max;
            this.online = online;
            this.sample = sample;
        }
    }

    public class ServerStatus : MCJson
    {
        public ServerStatusVersion version;
        public ServerStatusPlayerInfo players;

        public MCJsonString description;

        public string favicon;
    }
}
