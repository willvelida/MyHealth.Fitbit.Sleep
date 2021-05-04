﻿using AutoMapper;
using MyHealth.Fitbit.Sleep.Models;
using System.Diagnostics.CodeAnalysis;
using mdl = MyHealth.Common.Models;

namespace MyHealth.Fitbit.Sleep.Profiles
{
    [ExcludeFromCodeCoverage]
    public class SleepProfile : Profile
    {
        public SleepProfile()
        {
            CreateMap<SleepResponseObject, mdl.Sleep>()
                .ForMember(
                    dest => dest.SleepDate,
                    opt => opt.MapFrom(src => src.sleep[0].dateOfSleep))
                .ForMember(
                    dest => dest.EndTime,
                    opt => opt.MapFrom(src => src.sleep[0].endTime))
                .ForMember(
                    dest => dest.StartTime,
                    opt => opt.MapFrom(src => src.sleep[0].startTime))
                .ForMember(
                    dest => dest.MinutesAsleep,
                    opt => opt.MapFrom(src => src.summary.totalMinutesAsleep))
                .ForMember(
                    dest => dest.MinutesDeepSleep,
                    opt => opt.MapFrom(src => src.summary.stages.deep))
                .ForMember(
                    dest => dest.MinutesLightSleep,
                    opt => opt.MapFrom(src => src.summary.stages.light))
                .ForMember(
                    dest => dest.MinutesREMSleep,
                    opt => opt.MapFrom(src => src.summary.stages.rem))
                .ForMember(
                    dest => dest.MinutesAwake,
                    opt => opt.MapFrom(src => src.summary.stages.wake))
                .ForMember(
                    dest => dest.NumberOfAwakenings,
                    opt => opt.MapFrom(src => src.sleep[0].awakeningsCount))
                .ForMember(
                    dest => dest.TimeInBed,
                    opt => opt.MapFrom(src => src.summary.totalTimeInBed));
        }
    }
}
