import type { DomainsDto } from "@/types/domain";
import type { ColumnDef } from "@tanstack/react-table";
import { useNavigate } from "react-router-dom";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import { Button } from "@/components/ui/button"
import { MoreHorizontal } from "lucide-react"
import DialogDelete from "@/features/dialog-delete";
import { useState } from "react";
import { domain } from "@/lib/api";
import { toast } from "sonner";

// Update this to accept refresh callback
export const columns = (refreshCallback: () => void): ColumnDef<DomainsDto>[] => [
  {
    accessorKey: "name",
    header: "Name",
  },
  {
    accessorKey: "isActive",
    header: "Active",
  },
  {
    id: "actions",
    cell: ({ row }) => {
        const [isDeleteOpen, setIsDeleteOpen] = useState(false)
        const domainRow = row.original
        const navigate = useNavigate();

        const handleDelete = async () => {
          try {
            // Fix: await the promise
            await domain.delete(domainRow.id);
            toast.success("Domain deleted successfully");
            // Close the dialog first
            setIsDeleteOpen(false);
            // Then refresh the data
            refreshCallback();
          } catch (error) {
            toast.error("Failed to delete domain");
          }
        };

        return (
        <DropdownMenu>
          <DropdownMenuTrigger asChild className="absolute right-0 transform -translate-y-1/2">
            <Button variant="ghost" className="h-8 w-8 p-0">
              <span className="sr-only">Open menu</span>
              <MoreHorizontal className="h-4 w-4" />
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end">
            <DropdownMenuLabel>Actions</DropdownMenuLabel>
            <DropdownMenuSeparator />
            <DropdownMenuItem className="text-primary" onClick={() => navigate(`/dashboard/domains/${domainRow.id}`)}>
              View details
            </DropdownMenuItem>
              {/* Delete Button */}
              <DropdownMenuItem 
                className="cursor-pointer p-0 text-red-600"
                onSelect={(e) => e.preventDefault()}
              >
                <div className="w-full">
                  <DialogDelete
                    message="domain"
                    onSubmit={handleDelete}
                    open={isDeleteOpen}
                    onOpenChange={setIsDeleteOpen}
                  />
                </div>
              </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
        )
    },
  }
]