"use client";

import * as React from "react";
import {
  Tour,
  TourArrow,
  TourClose,
  TourDescription,
  TourFooter,
  TourHeader,
  TourNext,
  TourPortal,
  TourPrev,
  TourSpotlight,
  TourSpotlightRing,
  TourStep,
  TourStepCounter,
  TourTitle,
} from "@/components/ui/tour";
 

interface AppTourProps {
    open: boolean;
    setOpen: (open: boolean) => void;
}

export function AppTour({ open, setOpen }: AppTourProps) {
    return (
        <Tour
        open={open}
        onOpenChange={setOpen}
        stepFooter={
          <TourFooter>
            <div className="flex w-full items-center justify-between">
              <TourStepCounter />
              <div className="flex gap-2">
                <TourPrev />
                <TourNext />
              </div>
            </div>
          </TourFooter>
        }
      >
        <TourPortal>
          <TourSpotlight />
          <TourSpotlightRing />
          <TourStep target="#welcome-title" side="bottom" align="center">
            <TourHeader>
              <TourTitle>Welcome!</TourTitle>
              <TourDescription>
                Let's walk through the main features of your dashboard in just a
                few steps.
              </TourDescription>
            </TourHeader>
            <TourClose />
          </TourStep>
          <TourStep target="#feature-1" side="top" align="center">
            <TourArrow />
            <TourHeader>
              <TourTitle>Domains Dashboard</TourTitle>
              <TourDescription>
                View and manage all your DNS records in one place with our intuitive dashboard.
              </TourDescription>
            </TourHeader>
            <TourClose />
          </TourStep>
          <TourStep target="#feature-2" side="top" align="center">
            <TourArrow />
            <TourHeader>
              <TourTitle>Search DNS Records</TourTitle>
              <TourDescription>
                Use the search bar to quickly find specific DNS records by name or other criteria.
              </TourDescription>
            </TourHeader>
            <TourClose />
          </TourStep>
          <TourStep target="#feature-3" side="top" align="center" required>
            <TourArrow />
            <TourHeader>
              <TourTitle>Add DNS Record</TourTitle>
              <TourDescription>
                Use this feature to add new DNS records to your dashboard. This
                step is required to continue.
              </TourDescription>
            </TourHeader>
            <TourClose />
          </TourStep>
          <TourStep target="#feature-4" side="top" align="center">
            <TourArrow />
            <TourHeader>
              <TourTitle>Logout</TourTitle>
              <TourDescription>
                Click here to securely log out of your account when you're done.
              </TourDescription>
            </TourHeader>
            <TourClose />
          </TourStep>
          <TourStep target="#feature-5" side="top" align="center">
            <TourArrow />
            <TourHeader>
              <TourTitle>Take a Tour</TourTitle>
              <TourDescription>
                You can retake this tour anytime by clicking the "Take a Tour" button in the top right corner.
              </TourDescription>
            </TourHeader>
            <TourClose />
          </TourStep>
        </TourPortal>
      </Tour>
    );
}